using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF6.Banking.Persistence.Migrations
{
    public partial class AddedDefaultTenantAccountsAndPeople : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Name" },
                values: new object[] { 20, "X Bank" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "TenantId" },
                values: new object[] { 20, "Student Savings", 20 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "TenantId" },
                values: new object[] { 21, "Student Savings", 20 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "TenantId" },
                values: new object[] { 22, "Student Savings", 20 });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "AccountId", "Name" },
                values: new object[] { 20, 20, "Bill Gates" });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "AccountId", "Name" },
                values: new object[] { 21, 21, "Jeff Bezos" });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "AccountId", "Name" },
                values: new object[] { 22, 22, "Elon Musk" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
