using Identity.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data.EntityConfigs
{
    public sealed class RoleClaimConfig : IEntityTypeConfiguration<RoleClaimEntity>
    {
        public void Configure(EntityTypeBuilder<RoleClaimEntity> b)
        {
            b.ToTable("RoleClaims", "dbo");
            b.HasKey(x => x.Id).HasName("PK_RoleClaims");
            b.HasIndex(x => x.RoleId).HasDatabaseName("IX_RoleClaims_RoleId");
            b.HasOne(x => x.Role).WithMany(r => r.Claims).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
