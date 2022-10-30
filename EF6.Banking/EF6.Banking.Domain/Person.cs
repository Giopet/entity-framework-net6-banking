using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6.Banking.Domain
{
    /// <summary>
    /// Lets have a convention that a person can only have one account at a time
    /// </summary>
    public class Person : DomainModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Nullable, because is not required for a person to have an account
        /// </summary>
        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }

    }
}
