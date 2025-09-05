using Bank.Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken ct = default);
    Task<RefreshToken?> GetLatestAsync(Guid userId, Guid deviceId, Guid familyId, CancellationToken ct = default);
    Task<List<RefreshToken>> GetByFamilyAsync(Guid userId, Guid deviceId, Guid familyId, CancellationToken ct = default);
    Task RevokeFamilyAsync(Guid userId, Guid deviceId, Guid familyId, string reason, DateTime nowUtc, CancellationToken ct = default);
}
