using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities
{
    public sealed class Device
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; private set; } = default!;
        public string Platform { get; private set; } = default!;
        public string? Fingerprint { get; private set; }
        public DateTime CreatedAt { get; init; }
        public DateTime? LastSeenAt { get; private set; }
    }
}
