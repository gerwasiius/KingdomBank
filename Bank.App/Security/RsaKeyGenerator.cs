using Bank.App.Security.Interfaces;
using Bank.Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Security
{
    public sealed class RsaKeyGenerator : IKeyGenerator
    {
        public KeyAlgorithm Algorithm => KeyAlgorithm.RS256;

        public (byte[] privateKeyPem, Jwk publicJwk) Generate(string kid)
        {
            using var rsa = RSA.Create(3072);
            var priv = rsa.ExportPkcs8PrivateKey();
            var pem = PemEncoding.Write("PRIVATE KEY", priv);

            var p = rsa.ExportParameters(false);
            var n = B64Url(p.Modulus!);
            var e = B64Url(p.Exponent!);

            var jwk = new Jwk(Kty: "RSA", Kid: kid, Alg: "RS256", Use: "sig", N: n, E: e);
            return (System.Text.Encoding.ASCII.GetBytes(pem), jwk);
        }

        private static string B64Url(byte[] d) =>
            Convert.ToBase64String(d).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}
