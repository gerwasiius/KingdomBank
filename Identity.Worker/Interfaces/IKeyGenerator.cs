using Identity.Worker.Models;

namespace Identity.Worker.Interfaces
{
    public interface IKeyGenerator
    {
        KeyAlgorithm Algorithm { get; }
        (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid);
    }
}
