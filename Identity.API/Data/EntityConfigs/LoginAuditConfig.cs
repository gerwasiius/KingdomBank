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
    public sealed class LoginAuditConfig : IEntityTypeConfiguration<LoginAuditEntity>
    {
        public void Configure(EntityTypeBuilder<LoginAuditEntity> b)
        {
            b.ToTable("LoginAudit", "dbo");
            b.HasKey(x => x.Id).HasName("PK_LoginAudit");
            b.HasIndex(x => new { x.CreatedAt, x.UserId }).HasDatabaseName("IX_LoginAudit_Created_User");
        }
    }
}
