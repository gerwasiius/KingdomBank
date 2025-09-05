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
    public sealed class KeyRotationConfig : IEntityTypeConfiguration<KeyRotationEntity>
    {
        public void Configure(EntityTypeBuilder<KeyRotationEntity> b)
        {
            b.ToTable("KeyRotation", "dbo");
            b.HasKey(x => x.Id).HasName("PK_KeyRotation");
            b.Property(x => x.Id).ValueGeneratedOnAdd();
            b.Property(x => x.Kid).IsRequired().HasMaxLength(64);
            b.HasIndex(x => x.Kid).IsUnique().HasDatabaseName("UX_KeyRotation_Kid");
            b.Property(x => x.Alg).IsRequired().HasMaxLength(20);
            b.Property(x => x.Status).IsRequired().HasMaxLength(20);
            b.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
