using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.Entities
{
    public sealed class RoleClaimEntity
    {
        public long Id { get; set; }
        public Guid RoleId { get; set; }
        public string Type { get; set; } = default!;   // "perm"
        public string Value { get; set; } = default!;  // npr. "users.read"

        public RoleEntity Role { get; set; } = default!;
    }
}
