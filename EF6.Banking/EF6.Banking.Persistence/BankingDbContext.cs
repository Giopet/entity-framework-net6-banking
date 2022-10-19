using EF6.Banking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF6.Banking.Persistence
{
    /// <summary>
    /// Represents the connection to database
    /// </summary>
    public class BankingDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=Banking_EF6")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information) // Everything happens, is going to be seen.
                .EnableSensitiveDataLogging(); // Everything happens in the background that probably end-user must not see.
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Account> Accounts { get; set; } 
    }
}