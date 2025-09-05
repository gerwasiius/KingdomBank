using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface IJwtFactory
{
    string CreateAccessToken(IEnumerable<Claim> claims, string kid, DateTime expiresUtc);
}
