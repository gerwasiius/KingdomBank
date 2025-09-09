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
    public sealed class PasswordResetTokenConfig : IEntityTypeConfiguration<PasswordResetTokenEntity>
    {
        public void Configure(EntityTypeBuilder<PasswordResetTokenEntity> b)
        {
            b.ToTable("PasswordResetTokens", "dbo");
            b.HasKey(x => x.Id).HasName("PK_PasswordReset");
            b.HasIndex(x => new { x.UserId, x.ExpiresAt }).HasDatabaseName("IX_PasswordReset_User_Exp");
        }
    }
}
