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
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.HasIndex(i => new { i.Name, i.AccountId }).IsUnique();

            builder.HasData(
                    new Person
                    {
                        Id = 20,
                        Name = "Bill Gates",
                        AccountId = 20
                    },
                    new Person
                    {
                        Id = 21,
                        Name = "Jeff Bezos",
                        AccountId = 21
                    },
                    new Person
                    {
                        Id = 22,
                        Name = "Elon Musk",
                        AccountId = 22
                    }
                );
        }
    }
}
