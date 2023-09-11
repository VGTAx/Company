using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class rework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityUser<string>",
                keyColumn: "Id",
                keyValue: "d2a2170f-1a1b-4341-81d3-af3088a22402");

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e14bb3d5-59f5-4945-94a0-a9bed1e9a40b");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f26edcc9-bf28-4855-aa0f-481462aac9b8");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "116106a2-7cb1-4dd0-8e86-c86ee9524398");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ff409fbd-0aea-4c5d-acca-7c6e02f7445e");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f075da41-8929-4807-9dfa-b0b9a84de098");

            migrationBuilder.UpdateData(
                table: "IdentityRole<string>",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "d4f1bb5a-fafd-465f-a54f-2510bf379c58");

            migrationBuilder.InsertData(
                table: "IdentityUser<string>",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d2a2170f-1a1b-4341-81d3-af3088a22402", 0, "b5d448ea-0d80-44c3-95e5-5bdd0b9c05a2", "ApplicationUserModel", "putinvodkagta@yandex.by", true, true, null, null, "PUTINVODKAGTA@YANDEX.BY", "PUTINVODKAGTA@YANDEX.BY", "AQAAAAIAAYagAAAAEDHhCwj3vdAZ+8HkJt6+R/FthrKsnDZ3QAuDM36ysFiVoqMy2LIEyFmbdnYiMRKjmg==", null, false, "ddb1a014-e91f-49f0-815d-ee9505a09eac", false, "putinvodkagta@yandex.by" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 11, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "d2a2170f-1a1b-4341-81d3-af3088a22402" });
        }
    }
}
