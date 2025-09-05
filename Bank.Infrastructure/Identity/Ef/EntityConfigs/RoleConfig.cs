using Bank.Infrastructure.Identity.Ef.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.Identity.Ef.EntityConfigs
{
    public sealed class RoleConfig : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> b)
        {
            b.ToTable("Roles", "dbo");
            b.HasKey(x => x.Id).HasName("PK_Roles");
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UX_Roles_Name");
        }
    }
}
