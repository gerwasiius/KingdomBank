using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Interfaces
{
    public interface IPasswordHasher
    {
        byte[] Sha256(string raw);
    }
}
