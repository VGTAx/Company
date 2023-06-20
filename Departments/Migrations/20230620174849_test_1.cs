using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class test_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Departments",
                newName: "ID1");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "fe2ca491-f181-4994-b69a-40c96776a655");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID1",
                table: "Departments",
                newName: "ID");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6f20ab57-0a73-40f5-957e-7755ce4273c3");
        }
    }
}
