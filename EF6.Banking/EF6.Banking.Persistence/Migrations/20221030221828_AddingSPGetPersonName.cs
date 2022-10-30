using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF6.Banking.Persistence.Migrations
{
    public partial class AddingSPGetPersonName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE sp_GetAccountPerson @accountId int
                                    AS
                                    BEGIN
                                        SELECT * FROM People WHERE AccountId = @accountId
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[sp_GetAccountPerson]");
        }
    }
}
