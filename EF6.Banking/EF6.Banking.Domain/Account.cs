namespace EF6.Banking.Domain
{
    public class Account : DomainModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Naming Convention: EF infers that Tenant is the Table and Id is the foreignKey
        /// If this property is missing, then a nullable foreign key is creating on db.
        /// Putting "Id" in the end of the property name is the naming convention that EFCore needs to make it automatically foreign key.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Navigation Property: Automatically include the related data of table Tenant through this property
        /// </summary>
        public virtual Tenant Tenant { get; set; }

        public virtual List<Transaction> DebitTransactions { get; set; }

        public virtual List<Transaction> CreditTransactions { get; set; }

    }
}