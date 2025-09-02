using Bank.App.Security;
using Bank.App.Security.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bank.Workers
{
    public sealed class KeyRotationWorker : BackgroundService
    {
        private readonly ILogger<KeyRotationWorker> _log;
        private readonly IConfiguration _cfg;
        private readonly IEnumerable<IKeyGenerator> _generators;
        private readonly IKeyVault _vault;
        private readonly IServiceScopeFactory _scopeFactory; // ⬅️ umjesto IKeyRotationRepository

        public KeyRotationWorker(
            ILogger<KeyRotationWorker> log,
            IConfiguration cfg,
            IEnumerable<IKeyGenerator> generators,
            IKeyVault vault,
            IServiceScopeFactory scopeFactory)
        {
            _log = log;
            _cfg = cfg;
            _generators = generators;
            _vault = vault;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var algo = (_cfg.GetValue<string>("KeyManagement:Algorithm") ?? "RS256") == "ES256"
                ? KeyAlgorithm.ES256 : KeyAlgorithm.RS256;

            var rotationDays = _cfg.GetValue<int>("KeyManagement:RotationDays", 90);
            var graceDays = _cfg.GetValue<int>("KeyManagement:GraceDays", 30);
            var prefix = _cfg.GetValue<string>("KeyManagement:KidPrefix") ?? "bank";

            var generator = _generators.First(g => g.Algorithm == algo);

            _log.LogInformation("KeyRotationWorker started (alg={Alg}, rotationDays={R}, graceDays={G})",
                algo, rotationDays, graceDays);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IKeyRotationRepository>();

                    var active = await repo.GetCurrentActiveAsync(stoppingToken);
                    var now = DateTime.UtcNow;

                    // 1) Ako nema aktivnog ili je istekao rotacijski period → generiši novi ključ
                    var needsNew =
                        active is null ||
                        (now - active.CreatedAtUtc).TotalDays >= rotationDays;

                    if (needsNew)
                    {
                        var kid = $"{prefix}-{now:yyyyMMdd-HHmmss}";
                        var desc = new KeyDescriptor(kid, algo, now);
                        var (privPem, jwk) = generator.Generate(kid);

                        // Private ključ u vault (/keys/...), public JWK u fajl (ako je FileKeyVault)
                        await _vault.StorePrivateKeyAsync(desc, privPem, stoppingToken);
                        if (_vault is Bank.Infrastructure.Security.FileKeyVault fkv)
                            await fkv.StorePublicJwkAsync(desc, jwk, stoppingToken);

                        await repo.InsertAsync(desc, "active", stoppingToken);
                        _log.LogInformation("KeyRotation: created ACTIVE key {Kid}", kid);

                        if (active is not null)
                        {
                            await repo.UpdateStatusAsync(active.Kid, "grace", stoppingToken);
                            _log.LogInformation("KeyRotation: previous key {Kid} -> GRACE", active.Kid);
                        }
                    }

                    // 2) Sve GRACE ključeve koji su prešli (rotation + grace) proglasi EXPIRED
                    var grace = await repo.GetAllByStatusAsync("grace", stoppingToken);
                    foreach (var g in grace)
                    {
                        if ((now - g.CreatedAtUtc).TotalDays >= rotationDays + graceDays)
                        {
                            await repo.UpdateStatusAsync(g.Kid, "expired", stoppingToken);
                            _log.LogInformation("KeyRotation: key {Kid} -> EXPIRED", g.Kid);
                        }
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // graceful shutdown
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "KeyRotation tick failed");
                }

                // tick svake 5 minuta (slobodno promijeni u configu)
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
