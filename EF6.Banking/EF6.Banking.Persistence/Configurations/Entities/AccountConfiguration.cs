using EF6.Banking.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6.Banking.Persistence
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasMany(m => m.DebitTransactions)
                .WithOne(m => m.DebitAccount)
                .HasForeignKey(m => m.DebitAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Account cannot be removed if not all transactions that points are not removed as well

            //.ToTable("TableName") - if you have different name for the table on database than in class
            builder.HasMany(m => m.CreditTransactions)
                .WithOne(m => m.CreditAccount)
                .HasForeignKey(m => m.CreditAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(50);
            builder.HasIndex(i => i.Name); // If we search by the name, it should have high speed point for the data. Query runs quickly.

            // This is equivalent to modelBuilder inside on OnModelCreating() on BankingDbContext class. builder is equivalent to modelBuilder.Entity<Account>()
            builder.HasData(
                    new Account
                    {
                        Id = 20,
                        Name = "Student Savings",
                        TenantId = 20
                    },
                    new Account
                    {
                        Id = 21,
                        Name = "Student Savings",
                        TenantId = 20
                    },
                    new Account
                    {
                        Id = 22,
                        Name = "Student Savings",
                        TenantId = 20
                    }
                );
        }
    }
}
