using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF6.Banking.Persistence.Migrations
{
    public partial class AddingAccountDetailsViewAndEarlyTransactionFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[GetEarliestTransaction] (@accountId int)
                                    RETURNS datetime
                                    BEGIN
                                        DECLARE @result datetime
                                        SELECT TOP 1 @result = date FROM [dbo].[Transactions] ORDER BY Date
                                        RETURN @result
                                    END");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[AccountsPeopleTenants]
                                    AS 
                                    SELECT a.Name, p.Name AS PersonName, t.Name AS TenantName
                                    FROM dbo.Accounts AS a 
                                    LEFT OUTER JOIN dbo.People AS p ON a.Id = p.AccountId
                                    INNER JOIN dbo.Tenants AS t ON a.TenantId = t.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[AccountsPeopleTenants]");
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[GetEarliestTransaction]");
        }
    }
}
