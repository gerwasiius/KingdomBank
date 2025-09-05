using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.Entities
{
    public sealed class PasswordResetTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] TokenHash { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? ConsumedAt { get; set; }
    }
}
