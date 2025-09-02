using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Security.Interfaces
{
    public interface IKeyRotationRepository
    {
        Task InsertAsync(KeyDescriptor d, string status, CancellationToken ct = default);
        Task<KeyDescriptor?> GetCurrentActiveAsync(CancellationToken ct = default);
        Task<List<KeyDescriptor>> GetAllByStatusAsync(string status, CancellationToken ct = default);
        Task UpdateStatusAsync(string kid, string newStatus, CancellationToken ct = default);
    }
}
