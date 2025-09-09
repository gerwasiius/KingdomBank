using Identity.Worker.Data.Entities;
using Identity.Worker.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Worker.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        public DbSet<KeyRotationEntity> KeyRotation => Set<KeyRotationEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new KeyRotationConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
