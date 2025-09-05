using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.Entities
{
    public sealed class LoginAuditEntity
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string Result { get; set; } = default!; // Success/Fail/Lockout/ReuseDetected
        public string? Reason { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
