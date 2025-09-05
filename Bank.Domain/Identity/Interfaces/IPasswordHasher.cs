using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface IPasswordHasher
{
    (byte[] Hash, byte[] Salt) HashPassword(string password);
    bool Verify(string password, byte[] hash, byte[] salt);
}