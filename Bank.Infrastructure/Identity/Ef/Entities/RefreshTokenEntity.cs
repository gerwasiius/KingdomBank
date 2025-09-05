using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.Entities
{
    public sealed class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid FamilyId { get; set; }
        public byte[] TokenHash { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ReplacedById { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        public bool ReuseDetected { get; set; }

        public UserEntity User { get; set; } = default!;
        public DeviceEntity Device { get; set; } = default!;
    }
}
