using Bank.Workers.Models;

namespace Bank.Workers.Interfaces
{
    public interface IKeyRotationRepository
    {
        Task InsertAsync(KeyDescriptor d, string status, CancellationToken ct = default);
        Task<KeyDescriptor?> GetCurrentActiveAsync(CancellationToken ct = default);
        Task<List<KeyDescriptor>> GetAllByStatusAsync(string status, CancellationToken ct = default);
        Task UpdateStatusAsync(string kid, string newStatus, CancellationToken ct = default);
    }
}
