using Bank.App.Identity.Security.Models;
using Bank.Shared.Identity.Contracts.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Interfaces
{
    public interface IKeyVault
    {
        Task StorePrivateKeyAsync(KeyDescriptor descriptor, byte[] privateKeyPem, CancellationToken ct = default);
        Task<byte[]> GetPrivateKeyAsync(string kid, CancellationToken ct = default);
        Task<Jwk> GetPublicJwkAsync(KeyDescriptor descriptor, CancellationToken ct = default);
        Task<IReadOnlyList<string>> ListKidsAsync(CancellationToken ct = default);
    }
}
