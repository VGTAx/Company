using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class add_identity_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "78173e02-b818-4568-8913-7196c01a3f2c");

            migrationBuilder.InsertData(
                table: "IdentityRole<string>",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2", "8f0ef729-02c1-42e9-8324-72a17be884bd", "IdentityRole", "User", "USER" },
                    { "3", "09cc816d-4ff9-42bf-aaa4-fb6dab9a408e", "IdentityRole", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "fc6468b8-eba8-430f-8e0e-d6e708183708");
        }
    }
}
