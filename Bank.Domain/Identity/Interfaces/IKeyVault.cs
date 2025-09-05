using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface IKeyVault
{
    // Čuva/učitava privatne ključeve (npr. RSA/EC) za potpis JWT-a
    Task<byte[]> GetPrivateKeyAsync(string kid, CancellationToken ct = default);
    Task StorePrivateKeyAsync(string kid, byte[] key, CancellationToken ct = default);
}