// See https://aka.ms/new-console-template for more information

using EF6.Banking.Domain;
using EF6.Banking.Persistence;
using Microsoft.EntityFrameworkCore;

BankingDbContext context = new(); //old way: private static BankingDbContext context = new BankingDbContext();

/* Insert Operation Methods */
//await AddTenant(new Tenant { Name = "A Bank" });
//await AddAccountsWithTenantAddedFirst(new Tenant { Name = "B Bank" });
//await AddAccountWithTenantAddedIfNotExists(new Tenant { Name = "C Bank" });

/* Select Operation Methods */
//await LoadTenantsWithSimpleSelectQuery();

/* Filter Operation Methods */
await LoadTenantsWithFilter();


Console.WriteLine("Press Any Key for Application's Termination...");
Console.ReadKey();


async Task AddTenant(Tenant tenant) // old way: static
{
    context.Tenants.Add(tenant); // Tracked in memory until SaveChangesAsync called.
    await context.SaveChangesAsync(); // Generates the SQL, send it into the database and rollback if anything fails.
};

async Task AddAccountsWithTenantAddedFirst(Tenant tenant)
{
    await AddTenant(tenant);

    var accounts = new List<Account>
    {
        new Account
        {
            Name = "Savings",
            TenantId = tenant.Id, // Insert using id
        },
        new Account
        {
            Name = "Checking",
            Tenant = tenant, // Insert using navigation property
        }
    };

    await context.AddRangeAsync(accounts); // If tenant does not exist, will be added on db as well.
    await context.SaveChangesAsync();
}

async Task AddAccountWithTenantAddedIfNotExists(Tenant tenant)
{
    var account = new Account
    {
        Name = "Savings",
        Tenant = tenant,
    };

    await context.AddAsync(account); // If tenant does not exist, will be added on db as well.
    await context.SaveChangesAsync();
}

async Task LoadTenantsWithSimpleSelectQuery()
{
    // It needs 'ToListAsync()' to execute that query, enumerate it and send it back as objects.
    var tenants = await context.Tenants.ToListAsync();

    // If 'ToListAsync()' were not in the previous command, then the db connection would be remain open on the foreach loop - Expensive Operation and might create lock on table.
    foreach (var tenant in tenants)
    {
        Console.WriteLine($"{tenant.Id} - {tenant.Name}");
    }
}

async Task LoadTenantsWithFilter()
{
    // Parameterization is a good protection against sql injection attacks.
    Console.Write("Enter a Tenant Name (Or Part of): ");
    var tenantName = Console.ReadLine();

    var tenantsExactMatcheByName = await context.Tenants.Where(q => q.Name.Equals(tenantName)).ToListAsync();
    foreach (var tenant in tenantsExactMatcheByName)
    {
        Console.WriteLine($"{tenant.Id} - {tenant.Name}");
    }

    //var tenantsPartialMatchedByName = await context.Tenants.Where(q => q.Name.Contains(tenantName)).ToListAsync();
    var tenantsPartialMatchedByName = await context.Tenants.Where(q => EF.Functions.Like(q.Name, $"%{tenantName}%")).ToListAsync();
    foreach (var tenant in tenantsPartialMatchedByName)
    {
        Console.WriteLine($"{tenant.Id} - {tenant.Name}");
    }
}