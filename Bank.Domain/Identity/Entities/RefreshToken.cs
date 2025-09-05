using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid DeviceId { get; init; }
    public Guid FamilyId { get; init; }    // rotirajuća porodica RT-ova
    public byte[] TokenHash { get; private set; } = default!;
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid? ReplacedById { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedReason { get; private set; }
    public bool ReuseDetected { get; private set; }

    public void Revoke(string reason, DateTime nowUtc)
    {
        RevokedAt = nowUtc;
        RevokedReason = reason;
    }
}