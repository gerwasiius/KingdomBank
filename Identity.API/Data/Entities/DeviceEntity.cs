using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data.Entities
{
    public sealed class DeviceEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? DeviceName { get; set; }
        public byte[]? DeviceFingerprintHash { get; set; }
        public bool IsTrustedForMfa { get; set; }
        public DateTime FirstSeenAt { get; set; }
        public DateTime? LastSeenAt { get; set; }

        public UserEntity User { get; set; } = default!;
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
    }
}
