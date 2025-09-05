using Bank.App.Identity.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Services
{
    public sealed class SecureRandom : ISecureRandom
    {
        public string Base64Url(int bytes)
        {
            var b = RandomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
