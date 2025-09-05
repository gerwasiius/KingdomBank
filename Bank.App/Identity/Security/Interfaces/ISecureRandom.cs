using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Interfaces
{
    public interface ISecureRandom
    {
        string Base64Url(int bytes);
    }
}
