using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.Entities
{
    public sealed class UserClaimEntity
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;

        public UserEntity User { get; set; } = default!;
    }
}
