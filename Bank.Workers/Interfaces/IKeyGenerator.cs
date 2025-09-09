using Bank.Workers.Models;

namespace Bank.Workers.Interfaces
{
    public interface IKeyGenerator
    {
        KeyAlgorithm Algorithm { get; }
        (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid);
    }
}
