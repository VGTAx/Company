using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class rename_ID_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "fc6468b8-eba8-430f-8e0e-d6e708183708");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: "c3d54ff8-c325-417b-80a8-cfc2e96d1db7");
        }
    }
}
