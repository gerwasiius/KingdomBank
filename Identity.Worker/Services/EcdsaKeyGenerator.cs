using Identity.Worker.Interfaces;
using Identity.Worker.Models;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Worker.Services
{
    public sealed class EcdsaKeyGenerator : IKeyGenerator
    {
        public KeyAlgorithm Algorithm => KeyAlgorithm.ES256;

        public (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid)
        {
            using var ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var priv = ec.ExportPkcs8PrivateKey();
            var pem = PemEncoding.Write("PRIVATE KEY", priv);

            var p = ec.ExportParameters(false);
            var x = B64Url(p.Q.X!);
            var y = B64Url(p.Q.Y!);

            var jwk = new Jwk(Kty: "EC", Kid: kid, Alg: "ES256", Use: "sig", Crv: "P-256", X: x, Y: y);
            return (Encoding.ASCII.GetBytes(pem), jwk);
        }

        private static string B64Url(byte[] d) =>
            Convert.ToBase64String(d).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}
