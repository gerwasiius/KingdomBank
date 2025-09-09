using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data.Entities
{
    public sealed class UserRoleEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public UserEntity User { get; set; } = default!;
        public RoleEntity Role { get; set; } = default!;
    }
}
