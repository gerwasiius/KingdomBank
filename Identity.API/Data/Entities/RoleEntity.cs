using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data.Entities
{
    public sealed class RoleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsSystem { get; set; }

        public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
        public ICollection<RoleClaimEntity> Claims { get; set; } = new List<RoleClaimEntity>();
    }
}
