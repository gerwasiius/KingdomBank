using Identity.API.Data.Entities;
using Identity.API.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        public DbSet<KeyRotationEntity> KeyRotation => Set<KeyRotationEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();
        public DbSet<UserClaimEntity> UserClaims => Set<UserClaimEntity>();
        public DbSet<RoleClaimEntity> RoleClaims => Set<RoleClaimEntity>();
        public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
        public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
        public DbSet<LoginAuditEntity> LoginAudit => Set<LoginAuditEntity>();
        public DbSet<EmailVerificationTokenEntity> EmailVerificationTokens => Set<EmailVerificationTokenEntity>();
        public DbSet<PasswordResetTokenEntity> PasswordResetTokens => Set<PasswordResetTokenEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new KeyRotationConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());
            modelBuilder.ApplyConfiguration(new UserClaimConfig());
            modelBuilder.ApplyConfiguration(new RoleClaimConfig());
            modelBuilder.ApplyConfiguration(new DeviceConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfig());
            modelBuilder.ApplyConfiguration(new LoginAuditConfig());
            modelBuilder.ApplyConfiguration(new EmailVerificationTokenConfig());
            modelBuilder.ApplyConfiguration(new PasswordResetTokenConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
