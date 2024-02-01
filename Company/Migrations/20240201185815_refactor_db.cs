using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class refactor_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a" });

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7aec4b5f-e9a2-4aba-a203-1612363e7b19");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "2eeac276-5908-4a8f-a777-31b1e789de42");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "c708b786-dc14-4fad-aca4-cd2b579025e2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a",
                columns: new[] { "ConcurrencyStamp", "IsFirstLogin", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc6b7631-4cab-4e75-8fd2-2fc698d9a88b", true, "Admin", "AQAAAAIAAYagAAAAEGgizShuh0Axo+xn2WWlQI3O9/K/u32dMIlkgTokJQaL/eDgUPBwHXnYt8+lxHqtgQ==", "383d473a-c2c6-4c93-8028-1973b93d9e04" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "27e67289-0660-43ca-be1b-2630db1ae79a");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a52b2956-f596-4689-8890-f8c59382b97d");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "f73e71c7-cf21-473d-9a15-e1bedb79c61a");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a" },
                    { "2", "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a519bf6-e590-4dc1-94e6-7d7030b0afaf", null, "AQAAAAIAAYagAAAAEDgO6iPu1KkLoRgQsQCehs1MFX6THA6mdE56U1ilrNFUq12yec8KQQGbFWzTG1Fnww==", "e4f09c3c-65dd-4b5e-bfbf-9d67b0f41836" });
        }
    }
}
