namespace EF6.Banking.Domain
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }


        /// <summary>
        /// Naming Convention: EF infers that Tenant is the Table and Id is the foreignKey
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Navigation Property: Automatically include the related data of table Tenant through this property
        /// </summary>
        public virtual Tenant Tenant { get; set; }

    }
}