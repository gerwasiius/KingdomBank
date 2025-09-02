namespace Bank.Infrastructure.Security;

using Bank.Shared.Security;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Bank.App.Security;
using Microsoft.Extensions.Configuration; // Ensure this is present
using Microsoft.Extensions.Primitives;     // If needed for IChangeToken
using Bank.App.Security.Interfaces;

public sealed class FileKeyVault : IKeyVault
{
    private readonly string _root; // npr. /keys u kontejneru (mount volume)
    public FileKeyVault(IConfiguration cfg)
    {
        var section = cfg.GetSection("KeyVault:File:RootPath");
        _root = section?.Value ?? "/keys";
        Directory.CreateDirectory(_root);
    }

    public async Task StorePrivateKeyAsync(KeyDescriptor d, byte[] privateKeyPem, CancellationToken ct = default)
    {
        var dir = Path.Combine(_root, d.Kid);
        Directory.CreateDirectory(dir);
        await File.WriteAllBytesAsync(Path.Combine(dir, "key.pem"), privateKeyPem, ct);
        // public JWK već dolazi iz generatora → čuva ga caller (vidjet ćeš u workeru); ovdje pružamo i helper:
        // (nije obavezno da vault zna JWK; može ih davati i KeyManagementService)
    }

    public async Task<byte[]> GetPrivateKeyAsync(string kid, CancellationToken ct = default)
    {
        var path = Path.Combine(_root, kid, "key.pem");
        return await File.ReadAllBytesAsync(path, ct);
    }

    public async Task<Jwk> GetPublicJwkAsync(KeyDescriptor descriptor, CancellationToken ct = default)
    {
        var path = Path.Combine(_root, descriptor.Kid, "jwk.json");
        var json = await File.ReadAllTextAsync(path, ct);
        return JsonSerializer.Deserialize<Jwk>(json)!;
    }

    public Task<IReadOnlyList<string>> ListKidsAsync(CancellationToken ct = default)
    {
        var list = Directory.EnumerateDirectories(_root).Select(Path.GetFileName).Where(x => x != null)!.ToList();
        return Task.FromResult<IReadOnlyList<string>>(list);
    }

    // Helper – upiši JWK (pozvat ćemo iz workera nakon generisanja)
    public async Task StorePublicJwkAsync(KeyDescriptor d, Jwk jwk, CancellationToken ct = default)
    {
        var dir = Path.Combine(_root, d.Kid);
        Directory.CreateDirectory(dir);
        var json = JsonSerializer.Serialize(jwk);
        await File.WriteAllTextAsync(Path.Combine(dir, "jwk.json"), json, ct);
    }
}
