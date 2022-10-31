using EF6.Banking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF6.Banking.Persistence
{
    /// <summary>
    /// Represents the connection to database
    /// Inherits from AuditableBankingDbContext so can take all the extended functionality from this class
    /// </summary>
    public class BankingDbContext : AuditableBankingDbContext
    {
        /// <summary>
        /// For setting up the context
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=Banking_EF6")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information) // Everything happens, is going to be seen.
                .EnableSensitiveDataLogging(); // Everything happens in the background that probably end-user must not see.
        }

        /// <summary>
        /// For creation of a model or a migration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // We use Fluent API to define several rules, meaning that each line depends on the previous

            modelBuilder.Entity<Account>()
                .HasMany(m => m.DebitTransactions)
                .WithOne(m => m.DebitAccount)
                .HasForeignKey(m => m.DebitAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Account cannot be removed if not all transactions that points are not removed as well

            modelBuilder.Entity<Account>() //.ToTable("TableName") - if you have different name for the table on database than in class
                .HasMany(m => m.CreditTransactions)
                .WithOne(m => m.CreditAccount)
                .HasForeignKey(m => m.CreditAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountsPeopleTenantsView>().HasNoKey().ToView("AccountsPeopleTenants");

            // Kepp the order depending of the level of dependency between models
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());

        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<AccountsPeopleTenantsView> AccountsPeopleTenants { get; set; }
    }
}