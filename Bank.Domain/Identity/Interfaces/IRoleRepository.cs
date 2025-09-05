using Bank.Domain.Identity.Entities;
using System.Data;

namespace Bank.Domain.Identity.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(Role role, CancellationToken ct = default);
}
