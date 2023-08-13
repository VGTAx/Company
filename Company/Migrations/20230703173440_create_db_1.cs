using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company_.Migrations
{
    /// <inheritdoc />
    public partial class create_db_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Employees",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "39004f6a-07db-4125-a41a-c0b255bc19af");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ae254dfe-d00c-4bbb-aeb7-3d8745953bc4");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "b8eeceb9-e2da-4aae-ae75-efaec7986796");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "FullName");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ce97d922-c244-4fe4-a235-935a9f5b552a");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "65dd32a3-73db-4315-a7ed-734a8507e406");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "92d4474a-4382-4bfc-95e1-070c242a4844");
        }
    }
}
