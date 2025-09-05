using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities;
public sealed class SigningKey
{
    public Guid Id { get; init; }
    public string Kid { get; init; } = default!;    // key id za JWT kid header
    public DateTime NotBefore { get; init; }
    public DateTime NotAfter { get; init; }
    public bool IsRevoked { get; private set; }

    public void Revoke() => IsRevoked = true;
}