// See https://aka.ms/new-console-template for more information

using EF6.Banking.Domain;
using EF6.Banking.Persistence;
using Microsoft.EntityFrameworkCore;

BankingDbContext context = new(); //old way: private static BankingDbContext context = new BankingDbContext();

/* Insert Operation Methods */
//await AddTenant(new Tenant { Name = "A Bank" });
//await AddAccountsWithTenantAddedFirst(new Tenant { Name = "B Bank" });
//await AddAccountWithTenantAddedIfNotExists(new Tenant { Name = "C Bank" });
//await AddAccountWithTenantId();
//await AddTenantWithAccounts();
//await AddTransactions();
//await AddPerson();

/* Select Operation Methods */
//await LoadTenantsWithSimpleSelectQuery();

/* Filter Operation Methods */
//await LoadTenantsWithFilter();

/* Aggregate Filter Methods */
//await ExecuteAdditionalFilters();

/* Alternative Linq Syntax */
//await AlternativeLinqSyntax();

/* Update Methods */
//await SimpleUpdateTenant();
//await SimpleUpdateAccount();

/* Delete Methods */
//await SimpleTenantDelete();
//await DeleteTenantWithRelationship();

/* WithTracking vs WithoutTracking */
//await TrackingVsNoTracking();

/* Eager Loading - Including Related Data */
//await EagerLoadingRelatedData();

/* Projections to Data Transfer Objects or Anonymous Types */
//await SelectSingleProperty();
//await AnonymousTypeProjection();
//await StronglyTypeProjection();

/* Filtering on Related Data */
//await FilteringOnRelatedData();

/* Query View */
//await QueryView();

/* Query with Raw SQL */
//await RawSQLQuery();

/* Query Stored Procedures */
//await ExecuteStoredProcedure();

/* Raw SQL non-query Stored Procedures */
//await ExecuteNonQueryStoredProcedure();





Console.WriteLine("Press Any Key for Application's Termination...");
Console.ReadKey();


async Task AddTenant(Tenant tenant) // old way: private static
{
    await context.Tenants.AddAsync(tenant); // Tracked in memory until SaveChangesAsync called.
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

async Task ExecuteAdditionalFilters()
{
    var tenants = context.Tenants;
    var list = await tenants.ToListAsync();
    var first = await tenants.FirstAsync();
    var firstOrDefault = await tenants.FirstOrDefaultAsync();
    var single = await tenants.SingleAsync(); // possible exception
    var singleOrDefault = await tenants.SingleOrDefaultAsync(); // possible exception

    var count = await tenants.CountAsync();
    var longCount = await tenants.LongCountAsync();
    var min = await tenants.MinAsync(); // possible exception
    var max = await tenants.MaxAsync(); // possible exception

    // DbSet method that will execute - can be used with front-end passing this id
    var tenant = await tenants.FindAsync(1);
}

async Task AlternativeLinqSyntax()
{
    Console.Write("Enter a Tenant Name (Or Part of): ");
    var tenantName = Console.ReadLine();

    var tenants = await (from t in context.Tenants
                         where EF.Functions.Like(t.Name, $"%{tenantName}%") // or: t.Name.Contains()
                         select t).ToListAsync(); // make IEnumerable from IQuerable to have more methods

    foreach(var tenant in tenants)
    {
        Console.WriteLine($"{tenant.Id} - {tenant.Name}");
    }
}

async Task GetModel()
{
    // Retrieve Model
    var tenant = await context.Tenants.FindAsync(1);
    Console.WriteLine($"{tenant.Id} - {tenant.Name}");
}

/// <summary>
/// If update is already done , then if you update the same value no connection on db is opened
/// EF uses tracking to achieve this. Keeps in the memory what changes has occured.
/// </summary>
async Task SimpleUpdateTenant()
{
    // Retrieve Model
    var tenant = await context.Tenants.FindAsync(1);

    // Make Model Changes
    tenant.Name = "AA Bank";

    // Save Changes
    await context.SaveChangesAsync();

    await GetModel();
}

async Task SimpleUpdateAccount()
{
    // If we dont specify the id, an insert operation will happen and go add a row with new id.
    // If Id is wrong, will throw an exception.
    var account = new Account
    {
        Id = 1, 
        Name = "Money Market",
        TenantId = 1
    };

    context.Accounts.Update(account);

    await context.SaveChangesAsync();
}

async Task SimpleTenantDelete()
{
    var tenant = await context.Tenants.FindAsync(2);
    context.Tenants.Remove(tenant);
    await context.SaveChangesAsync();
}

/// <summary>
/// When we have related records delete method becomes a bit more sensitive.
/// Cascade delete: if record with the primary key is removed, 
/// then every other record that has foreign key to this one will also be deleted.
/// For changing cascade setting: on table constraints -> ForeignKey -> onDelete -> choose ReferentialAction.
/// It may be dangerous on circural references.
/// </summary>
async Task DeleteTenantWithRelationship()
{
    var tenant = await context.Tenants.FindAsync(1);
    context.Tenants.Remove(tenant);
    await context.SaveChangesAsync();
}

/// <summary>
/// The advantage to not tracking is that releases memory a bit more and speed up performance.
/// If you are retrieving 1000 records with tracking, EFCore will have to monitoring all these records on one request. Imagine in more requests.
/// No Tracking is useful in a simple request like a readonly list. For example list data from the database to parse them as view on the user.
/// But if you need to make changes on that list then data must be tracking.
/// 
/// There are times that might be concurrency issues when have already saved some changes 
/// and try to save them again accidentally and getting an error saying is already being tracked by EFCore.
/// So sometimes records should be released from being tracked.
/// </summary>
async Task TrackingVsNoTracking()
{
    // We could use Find() instead of First(), but AsNoTracking() doesn't work with Find().
    var tracking = await context.Accounts.FirstOrDefaultAsync(q => q.Id == 1);
    var noTracking = await context.Accounts.AsNoTracking().FirstOrDefaultAsync(q => q.Id == 2); // Is not tracking in memory

    tracking.Name = "E Bank";
    noTracking.Name = "F Bank"; // It is not changed on database

    var entriesBeforeSave = context.ChangeTracker.Entries();

    await context.SaveChangesAsync();

    var entriesAfterSave = context.ChangeTracker.Entries();
}

/// <summary>
/// A scenario where tenantId and name of account sent to back-end
/// </summary>
async Task AddAccountWithTenantId()
{
    var account = new Account
    {
        Name = "Checking",
        TenantId = 2,
    };

    await context.AddAsync(account); // If tenantId does not exist, will throw an exception.
    await context.SaveChangesAsync();
}

async Task AddTenantWithAccounts()
{
    var accounts = new List<Account>
    {
        new Account
        {
            Name = "Checking"
        },
        new Account
        {
            Name = "Savings"
        },
    };

    var tenant = new Tenant { Name = "I Bank" ,Accounts = accounts };

    await context.AddAsync(tenant);
    await context.SaveChangesAsync();
}

async Task AddTransactions()
{
    var transactions = new List<Transaction>
    {
        new Transaction
        {
            DebitAccountId = 3, CreditAccountId = 2, Date = new DateTime(2022,10,30)
        },
        new Transaction
        {
            DebitAccountId = 8, CreditAccountId = 9, Date = DateTime.Now
        },
        new Transaction
        {
            DebitAccountId = 9, CreditAccountId = 8, Date = DateTime.Now
        }
    };

    await context.AddRangeAsync(transactions);
    await context.SaveChangesAsync();
}

async Task AddPerson()
{
    var person1 = new Person { Name = "Gio Pet", AccountId = 2 };

    await context.AddAsync(person1);

    var person2 = new Person { Name = "Mona Lisa" }; // Doesn't have any account

    await context.AddAsync(person2);

    await context.SaveChangesAsync();
}

async Task EagerLoadingRelatedData()
{
    // Get many related models: Tenants -> Accounts
    var tenants = await context.Tenants.Include(q => q.Accounts).ToListAsync();

    // Get one related model: Account -> Person
    var account = await context.Accounts.Include(q => q.Person).FirstOrDefaultAsync(q => q.Id == 2);

    // Get 'Grand Children' related model: Account -> Transactions -> Debit/Credit Account
    var accountsWithTransactions = await context.Accounts
        .Include(q => q.DebitTransactions).ThenInclude(q => q.CreditAccount).ThenInclude(q => q.Person)
        .Include(q => q.CreditTransactions).ThenInclude(q => q.DebitAccount).ThenInclude(q => q.Person)
        .FirstOrDefaultAsync(q => q.Id == 2);

    // Get Includes with filters
    /// The result is not correct, it has accounts without Credit transactions. Be careful while filtering and how that is translating into sql queries.
    var accountsWithCreditTransactions = await context.Accounts
        .Where(q => q.CreditTransactions.Count > 0) // if you have written q.CreditTransactions.Count == null would be false because EF doesnt know anything about lists
        .Include(q => q.Person)
        .ToListAsync();

    /// The result is correct, but not the most optimized way.
    var accountsWithCreditTransactionsCorrect = (await context.Accounts.Include(q => q.Person).ToListAsync())
        .Where(a => a.CreditTransactions is not null);
}

async Task SelectSingleProperty()
{
    var accounts = await context.Accounts.Select(q => q.Name).ToListAsync();
}

async Task AnonymousTypeProjection()
{
    var anonymousType = await context.Accounts
        .Include(q => q.Person)
        .Select(q => new 
        {
            AccountName = q.Name, 
            PersonName = q.Person.Name 
        })
        .ToListAsync();

    foreach(var item in anonymousType)
    {
        Console.WriteLine($"{nameof(Account)}: {item.AccountName} | {nameof(Person)}: {item.PersonName}");
    }
}

async Task StronglyTypeProjection()
{
    var accountDetails = await context.Accounts
        .Include(q => q.Person)
        .Include(q => q.Tenant)
        .Select(q => new AccountDetail
        {
            Name = q.Name,
            PersonName = q.Person.Name,
            TenantName = q.Tenant.Name
        })
        .ToListAsync();

    foreach (var accountDetail in accountDetails)
    {
        Console.WriteLine($"{nameof(Account)}: {accountDetail.Name} | {nameof(Person)}: {accountDetail.PersonName} | {nameof(Tenant)}: {accountDetail.TenantName}");
    }
}

async Task FilteringOnRelatedData()
{
    // Include the accounts to view their info.
    var tenants = await context.Tenants.Where(q => q.Accounts.Any(a => a.Name.Contains("Checking"))).ToListAsync();
}

async Task QueryView()
{
    var details = await context.AccountsPeopleTenants.ToListAsync();
}

/// <summary>
/// If you have to parse parameters in raw sql statement, use FromSqlInterpolated() method
/// </summary>
async Task RawSQLQuery()
{
    // It must return all the properties of the returned type (*) and the names must match
    // It passes the literal value into the query - Bad practise, Potential sql injection
    var name = "Savings";
    var account1 = await context.Accounts.FromSqlRaw($"SELECT * FROM Accounts WHERE name = '{name}'").ToListAsync();

    // Parameterizing the query, that's why we dont need to put '' on the name
    var account2 = await context.Accounts.FromSqlInterpolated($"SELECT * FROM Accounts WHERE name = {name}").ToListAsync();
}

async Task ExecuteStoredProcedure()
{
    var accounId = 2;

    // Correct way of using FromSqlRaw() method by parsing parameters
    var result = await context.People.FromSqlRaw("EXEC dbo.sp_GetAccountPerson {0}", accounId).ToListAsync();
}

async Task ExecuteNonQueryStoredProcedure()
{
    var accounId = 5;
    var affected = await context.Database.ExecuteSqlRawAsync("EXEC dbo.sp_DeleteAccountById {0}", accounId);

    //if (affected > 1) // In this way you understand if an execution was successful.

    var accounId2 = 5;
    var affected2 = await context.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sp_DeleteAccountById {accounId}");
}











