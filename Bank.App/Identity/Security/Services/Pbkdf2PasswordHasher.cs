using Bank.App.Identity.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Services
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
