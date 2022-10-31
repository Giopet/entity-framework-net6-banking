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
