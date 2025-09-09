using Identity.API.Models;

namespace Identity.API.Interfaces
{
    public interface IKeyGenerator
    {
        KeyAlgorithm Algorithm { get; }
        (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid);
    }
}
