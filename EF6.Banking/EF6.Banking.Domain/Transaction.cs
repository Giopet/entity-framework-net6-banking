using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6.Banking.Domain
{
    /// <summary>
    /// We broke naming convention here, so we put some rules on model creating to define the foreign keys.
    /// </summary>
    public class Transaction : DomainModel
    {
        public int DebitAccountId { get; set; }

        public virtual Account DebitAccount { get; set; }

        public int CreditAccountId { get; set; }

        public virtual Account CreditAccount { get; set; }

        public DateTime Date { get; set; }

    }
}
