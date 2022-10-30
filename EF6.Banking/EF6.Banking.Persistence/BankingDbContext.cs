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
        // For setting up the context
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=Banking_EF6")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information) // Everything happens, is going to be seen.
                .EnableSensitiveDataLogging(); // Everything happens in the background that probably end-user must not see.
        }

        // For creation of a model or a migration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // We use Fluent API to define several rules, meaning that each line depends on the previous

            modelBuilder.Entity<Account>()
                .HasMany(m => m.DebitTransactions)
                .WithOne(m => m.DebitAccount)
                .HasForeignKey(m => m.DebitAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Account cannot be removed if not all transactions that points are not removed as well

            modelBuilder.Entity<Account>()
                .HasMany(m => m.CreditTransactions)
                .WithOne(m => m.CreditAccount)
                .HasForeignKey(m => m.CreditAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }
    }
}