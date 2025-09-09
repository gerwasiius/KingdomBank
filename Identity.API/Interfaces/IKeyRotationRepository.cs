using Identity.API.Models;

namespace Identity.API.Interfaces
{
    public interface IKeyRotationRepository
    {
        Task InsertAsync(KeyDescriptor d, string status, CancellationToken ct = default);
        Task<KeyDescriptor?> GetCurrentActiveAsync(CancellationToken ct = default);
        Task<List<KeyDescriptor>> GetAllByStatusAsync(string status, CancellationToken ct = default);
        Task UpdateStatusAsync(string kid, string newStatus, CancellationToken ct = default);
    }
}
