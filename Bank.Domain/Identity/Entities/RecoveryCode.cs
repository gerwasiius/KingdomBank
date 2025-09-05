using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities;
public sealed class RecoveryCode
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public byte[] CodeHash { get; init; } = default!;
    public bool Used { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UsedAt { get; private set; }

    public void MarkUsed(DateTime whenUtc)
    {
        Used = true;
        UsedAt = whenUtc;
    }
}