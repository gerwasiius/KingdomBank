using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bank.App.Identity.Security.Interfaces;
using Bank.App.Identity.Security.Models;
using Bank.Infrastructure.Identity.Ef;
using Bank.Infrastructure.Identity.Ef.Entities;
using Microsoft.EntityFrameworkCore;         // <- EF Core async + ExecuteUpdate
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bank.App.Identity.Security.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly BankDbContext _db;
        private readonly IKeyVault _vault;
        private readonly IKeyRotationRepository _keyRepo;
        private readonly TokenOptions _options;
        private readonly IPasswordHasher _hasher;
        private readonly ISecureRandom _rng;

        public TokenService(
            BankDbContext db,
            IKeyVault vault,
            IKeyRotationRepository keyRepo,
            IOptions<TokenOptions> opt,
            IPasswordHasher hasher,
            ISecureRandom rng)
        {
            _db = db; _vault = vault; _keyRepo = keyRepo; _options = opt.Value; _hasher = hasher; _rng = rng;
        }

        public async Task<IssuedTokenPair> IssueAsync(TokenIssueRequest request, CancellationToken ct = default)
        {
            // 1) Signing ključ
            var active = await _keyRepo.GetCurrentActiveAsync(ct)
                         ?? throw new InvalidOperationException("No active signing key");
            var pem = await _vault.GetPrivateKeyAsync(active.Kid, ct);
            var signing = CreateSigningCredentials(active, pem);

            // 2) Access JWT
            var now = DateTime.UtcNow;
            var accessExpires = now.AddMinutes(_options.AccessMinutes);
            var jti = Guid.NewGuid().ToString("N");
            var sid = request.SessionId ?? Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
                new(JwtRegisteredClaimNames.Jti, jti),
                new("sid", sid),
                new("amr", request.AmrMfa ? "mfa" : "pwd"),
                new(JwtRegisteredClaimNames.Iat, Epoch(now), ClaimValueTypes.Integer64)
            };
            foreach (var r in request.Roles) claims.Add(new Claim(ClaimTypes.Role, r));
            if (request.Permissions.Any())
                claims.Add(new Claim("permissions", string.Join(' ', request.Permissions)));

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: accessExpires,
                signingCredentials: signing);
            jwt.Header["kid"] = active.Kid;

            var access = new JwtSecurityTokenHandler().WriteToken(jwt);

            // 3) Refresh (rotirajuća familija + device binding)
            var familyId = Guid.NewGuid();
            var refreshRaw = _rng.Base64Url(64);
            var refreshHash = _hasher.Sha256(refreshRaw);

            var rt = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DeviceId = request.DeviceId,
                FamilyId = familyId,
                TokenHash = refreshHash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(_options.RefreshDays),
                ReuseDetected = false
            };

            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync(ct);

            return new IssuedTokenPair
            {
                AccessToken = access,
                AccessExpiresAt = accessExpires,
                RefreshToken = refreshRaw,
                RefreshExpiresAt = rt.ExpiresAt,
                FamilyId = familyId
            };
        }

        public async Task<IssuedTokenPair> RefreshAsync(RefreshRequest request, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            // SERIALIZABLE transakcija da spriječimo duple rotacije u race-u
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);

            var tokens = await _db.RefreshTokens
                .Where(x => x.UserId == request.UserId
                         && x.DeviceId == request.DeviceId
                         && x.FamilyId == request.FamilyId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            if (tokens.Count == 0)
                throw new SecurityTokenException("No refresh family");

            var latest = tokens.First(); // jedini dozvoljeni ispravan kandidat
            var providedHash = _hasher.Sha256(request.ProvidedRefreshToken);
            var matched = tokens.FirstOrDefault(t => t.TokenHash.SequenceEqual(providedHash));

            // 1) Reuse: token ne postoji u familiji ili nije najnoviji => revoke cijelu familiju
            if (matched == null || matched.Id != latest.Id)
            {
                foreach (var t in tokens)
                {
                    t.ReuseDetected = true;
                    t.RevokedAt ??= now;
                    t.RevokedReason ??= "reuse-detected";
                }
                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                throw new SecurityTokenException("Refresh reuse detected");
            }

            if (matched.RevokedAt != null || matched.ExpiresAt <= now)
                throw new SecurityTokenException("Refresh invalid/expired");

            // 2) Rotacija: označi stari i kreiraj novi u istoj familiji
            var newTokenRaw = _rng.Base64Url(64);
            var newTokenHash = _hasher.Sha256(newTokenRaw);

            var newRt = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = matched.UserId,
                DeviceId = matched.DeviceId,
                FamilyId = matched.FamilyId,
                TokenHash = newTokenHash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(_options.RefreshDays)
            };

            matched.ReplacedById = newRt.Id;
            // (opcionalno) odmah “revokaj” stari da ne bude ponovo upotrebljiv ni u istom procesu
            matched.RevokedAt = now;
            matched.RevokedReason ??= "rotated";

            _db.RefreshTokens.Add(newRt);

            // 3) Novi access
            var active = await _keyRepo.GetCurrentActiveAsync(ct)
                         ?? throw new InvalidOperationException("No active key");
            var pem = await _vault.GetPrivateKeyAsync(active.Kid, ct);
            var signing = CreateSigningCredentials(active, pem);

            var accessExpires = now.AddMinutes(_options.AccessMinutes);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim("sid", Guid.NewGuid().ToString("N")),
                new Claim("amr", "pwd"), // TODO: ako želiš zadržati MFA, prenesi iz sesije
                new Claim(JwtRegisteredClaimNames.Iat, Epoch(now), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: accessExpires,
                signingCredentials: signing);
            jwt.Header["kid"] = active.Kid;

            var access = new JwtSecurityTokenHandler().WriteToken(jwt);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return new IssuedTokenPair
            {
                AccessToken = access,
                AccessExpiresAt = accessExpires,
                RefreshToken = newTokenRaw,
                RefreshExpiresAt = newRt.ExpiresAt,
                FamilyId = newRt.FamilyId
            };
        }

        public async Task RevokeAsync(RevokeRequest request, CancellationToken ct = default)
        {
            var q = _db.RefreshTokens.Where(x => x.UserId == request.UserId);
            if (request.DeviceId is Guid did) q = q.Where(x => x.DeviceId == did);
            if (request.FamilyId is Guid fid) q = q.Where(x => x.FamilyId == fid);

            var now = DateTime.UtcNow;
            await q.ExecuteUpdateAsync(s => s
                .SetProperty(x => x.RevokedAt, now)
                .SetProperty(x => x.RevokedReason, "revoked-by-user"), ct);
        }

        private static SigningCredentials CreateSigningCredentials(KeyDescriptor desc, byte[] pem)
        {
            // Namjerno BEZ 'using' — ključ mora preživjeti do WriteToken()
            if (desc.Algorithm == KeyAlgorithm.ES256)
            {
                var ecdsa = ECDsa.Create();
                ecdsa.ImportFromPem(Encoding.ASCII.GetString(pem));
                return new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256);
            }
            else
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(Encoding.ASCII.GetString(pem));
                return new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            }
        }

        private static string Epoch(DateTime dt) => ((long)(dt - DateTime.UnixEpoch).TotalSeconds).ToString();
    }
}
