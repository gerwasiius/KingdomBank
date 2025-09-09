using Identity.API.Models;

namespace Identity.API.Interfaces
{
    public interface IKeyVault
    {
        Task StorePrivateKeyAsync(KeyDescriptor descriptor, byte[] privateKeyPem, CancellationToken ct = default);
        Task<byte[]> GetPrivateKeyAsync(string kid, CancellationToken ct = default);
        Task<Jwk> GetPublicJwkAsync(KeyDescriptor descriptor, CancellationToken ct = default);
        Task<IReadOnlyList<string>> ListKidsAsync(CancellationToken ct = default);
    }
}
