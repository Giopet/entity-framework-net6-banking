namespace EF6.Banking.Domain
{
    public class Tenant : DomainModel
    {
        public string Name { get; set; }

        public List<Account> Accounts { get; set; }
    }
}