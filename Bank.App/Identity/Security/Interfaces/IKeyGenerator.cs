using Bank.App.Identity.Security.Models;
using Bank.Shared.Identity.Contracts.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Interfaces
{
    public interface IKeyGenerator
    {
        KeyAlgorithm Algorithm { get; }
        (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid);
    }
}
