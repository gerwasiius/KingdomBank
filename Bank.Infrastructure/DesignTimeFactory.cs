using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseSqlServer("Server=localhost,14333;Database=BankDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;")
                .Options;
            return new BankDbContext(options);
        }
    }
}
