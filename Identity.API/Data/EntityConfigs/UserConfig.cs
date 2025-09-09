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
    public sealed class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> b)
        {
            b.ToTable("Users", "dbo");
            b.HasKey(x => x.Id).HasName("PK_Users");
            b.Property(x => x.Username).IsRequired().HasMaxLength(50);
            b.Property(x => x.Email).IsRequired().HasMaxLength(320);
            b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);

            b.HasIndex(x => x.Username).IsUnique().HasDatabaseName("UX_Users_Username");
            b.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UX_Users_Email");
        }
    }
}
