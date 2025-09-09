using Identity.API.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Services
{
    public sealed class Pbkdf2PasswordHasher : IPasswordHasher
    {
        public byte[] Sha256(string raw)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        }
    }
}
