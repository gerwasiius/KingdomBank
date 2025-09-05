using Bank.Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;

public interface IKeyRotationRepository
{
    Task<SigningKey?> GetActiveSigningKeyAsync(CancellationToken ct = default);
    Task<List<SigningKey>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SigningKey key, CancellationToken ct = default);
    Task RevokeAsync(Guid keyId, CancellationToken ct = default);
}