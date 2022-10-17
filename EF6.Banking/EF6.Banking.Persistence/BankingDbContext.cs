using EF6.Banking.Domain;
using Microsoft.EntityFrameworkCore;

namespace EF6.Banking.Persistence
{
    /// <summary>
    /// Represents the connection to database
    /// </summary>
    public class BankingDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=Banking_EF6");
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Account> Accounts { get; set; } 
    }
}