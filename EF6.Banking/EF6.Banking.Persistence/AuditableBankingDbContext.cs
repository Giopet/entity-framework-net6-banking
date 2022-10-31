using EF6.Banking.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6.Banking.Persistence
{
    /// <summary>
    /// Abstract to not be inherited
    /// </summary>
    public abstract class AuditableBankingDbContext : DbContext
    {
        /// <summary>
        /// Returns 1 if it is successful
        /// Returns less than 1 or 0 if it is failed
        /// 
        /// It is an extension of SaveChangesAsync - A method before the real method - A Wrapper
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(string username)
        {
            // Gives us the entries are saved on the memory and are ready to be saved on db
            var entries = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var auditableModel = (DomainModel)entry.Entity;
                auditableModel.ModifiedDate = DateTime.Now;
                auditableModel.ModifiedBy = username;

                if (entry.State == EntityState.Added)
                {
                    auditableModel.CreatedDate = DateTime.Now;
                    auditableModel.CreatedBy = username;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
