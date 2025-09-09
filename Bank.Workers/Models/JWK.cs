namespace Bank.Workers.Models;

public sealed record JwkSet(Jwk[] Keys);
public sealed record Jwk(
    string Kty,   // "RSA" ili "EC"
    string Kid,   // key id
    string Alg,   // "RS256" ili "ES256"
    string Use,   // "sig"
    string? N = null, // RSA modulus (base64url)
    string? E = null, // RSA exponent (base64url)
    string? Crv = null, // EC curve "P-256"
    string? X = null,   // EC X (base64url)
    string? Y = null    // EC Y (base64url)
);