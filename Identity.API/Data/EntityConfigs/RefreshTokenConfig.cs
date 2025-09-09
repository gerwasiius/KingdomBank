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
    public sealed class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> b)
        {
            b.ToTable("RefreshTokens", "dbo");
            b.HasKey(x => x.Id).HasName("PK_RefreshTokens");

            b.Property(x => x.TokenHash).IsRequired();
            b.HasIndex(x => new { x.UserId, x.DeviceId, x.FamilyId, x.ExpiresAt })
             .HasDatabaseName("IX_RT_User_Device_Family_Exp");

            b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Device).WithMany(d => d.RefreshTokens).HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
