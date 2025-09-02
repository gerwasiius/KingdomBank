using Bank.App.Security;
using Bank.App.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Workers
{
    public sealed class KeyRotationWorker : BackgroundService
    {
        private readonly ILogger<KeyRotationWorker> _log;
        private readonly IConfiguration _cfg;
        private readonly IEnumerable<IKeyGenerator> _generators;
        private readonly IKeyVault _vault;
        private readonly IKeyRotationRepository _repo;  

        public KeyRotationWorker(
            ILogger<KeyRotationWorker> log,
            IConfiguration cfg,
            IEnumerable<IKeyGenerator> generators,
            IKeyVault vault,
            IKeyRotationRepository repo)
        {
            _log = log; _cfg = cfg; _generators = generators; _vault = vault; _repo = repo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var algo = (_cfg.GetValue<string>("KeyManagement:Algorithm") ?? "RS256") == "ES256"
                ? KeyAlgorithm.ES256 : KeyAlgorithm.RS256;
            var rotationDays = _cfg.GetValue<int>("KeyManagement:RotationDays", 90);
            var graceDays = _cfg.GetValue<int>("KeyManagement:GraceDays", 30);
            var prefix = _cfg.GetValue<string>("KeyManagement:KidPrefix") ?? "bank";

            var generator = _generators.First(g => g.Algorithm == algo);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var active = await _repo.GetCurrentActiveAsync(stoppingToken);
                    var now = DateTime.UtcNow;

                    if (active is null || (now - active.CreatedAtUtc).TotalDays >= rotationDays)
                    {
                        var kid = $"{prefix}-{now:yyyyMMdd-HHmmss}";
                        var desc = new KeyDescriptor(kid, algo, now);
                        var (privPem, jwk) = generator.Generate(kid);

                        await _vault.StorePrivateKeyAsync(desc, privPem, stoppingToken);
                        if (_vault is Infrastructure.Security.FileKeyVault fkv)
                            await fkv.StorePublicJwkAsync(desc, jwk, stoppingToken);

                        await _repo.InsertAsync(desc, "active", stoppingToken);
                        _log.LogInformation("KeyRotation: created ACTIVE key {Kid}", kid);

                        if (active is not null)
                        {
                            await _repo.UpdateStatusAsync(active.Kid, "grace", stoppingToken);
                            _log.LogInformation("KeyRotation: previous key {Kid} -> GRACE", active.Kid);
                        }
                    }

                    var grace = await _repo.GetAllByStatusAsync("grace", stoppingToken);
                    foreach (var g in grace)
                    {
                        if ((now - g.CreatedAtUtc).TotalDays >= rotationDays + graceDays)
                        {
                            await _repo.UpdateStatusAsync(g.Kid, "expired", stoppingToken);
                            _log.LogInformation("KeyRotation: key {Kid} -> EXPIRED", g.Kid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "KeyRotation tick failed");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
