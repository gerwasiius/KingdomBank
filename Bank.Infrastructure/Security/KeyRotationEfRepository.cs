using Bank.App.Security;
using Bank.App.Security.Interfaces;
using Bank.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Security
{
    public sealed class KeyRotationEfRepository : IKeyRotationRepository
    {
        private readonly BankDbContext _db;
        public KeyRotationEfRepository(BankDbContext db) => _db = db;

        public async Task InsertAsync(KeyDescriptor d, string status, CancellationToken ct = default)
        {
            var row = new KeyRotationEntity
            {
                Kid = d.Kid,
                Alg = d.Algorithm == KeyAlgorithm.ES256 ? "ES256" : "RS256",
                Status = status,
                CreatedAt = DateTime.UtcNow,
                ActivatedAt = status == "active" ? DateTime.UtcNow : null
            };
            _db.KeyRotation.Add(row);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<KeyDescriptor?> GetCurrentActiveAsync(CancellationToken ct = default)
        {
            var row = await _db.KeyRotation.Where(x => x.Status == "active")
                                           .OrderByDescending(x => x.ActivatedAt)
                                           .FirstOrDefaultAsync(ct);
            if (row == null) return null;
            var alg = row.Alg == "ES256" ? KeyAlgorithm.ES256 : KeyAlgorithm.RS256;
            return new KeyDescriptor(row.Kid, alg, row.CreatedAt);
        }

        public async Task<List<KeyDescriptor>> GetAllByStatusAsync(string status, CancellationToken ct = default)
        {
            var rows = await _db.KeyRotation.Where(x => x.Status == status)
                                            .OrderBy(x => x.CreatedAt)
                                            .ToListAsync(ct);
            return rows.Select(r =>
                new KeyDescriptor(r.Kid, r.Alg == "ES256" ? KeyAlgorithm.ES256 : KeyAlgorithm.RS256, r.CreatedAt)
            ).ToList();
        }

        public async Task UpdateStatusAsync(string kid, string newStatus, CancellationToken ct = default)
        {
            var row = await _db.KeyRotation.FirstOrDefaultAsync(x => x.Kid == kid, ct);
            if (row == null) return;
            row.Status = newStatus;
            var now = DateTime.UtcNow;
            row.RetiredAt = (newStatus is "expired" or "revoked") ? now : null;
            row.ActivatedAt = (newStatus == "active") ? now : row.ActivatedAt;
            await _db.SaveChangesAsync(ct);
        }
    }
}
