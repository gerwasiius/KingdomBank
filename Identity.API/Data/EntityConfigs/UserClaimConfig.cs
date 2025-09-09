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
    public sealed class UserClaimConfig : IEntityTypeConfiguration<UserClaimEntity>
    {
        public void Configure(EntityTypeBuilder<UserClaimEntity> b)
        {
            b.ToTable("UserClaims", "dbo");
            b.HasKey(x => x.Id).HasName("PK_UserClaims");
            b.HasIndex(x => x.UserId).HasDatabaseName("IX_UserClaims_UserId");
            b.HasOne(x => x.User).WithMany(u => u.Claims).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
