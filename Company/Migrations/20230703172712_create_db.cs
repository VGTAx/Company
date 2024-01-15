using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.Migrations
{
  /// <inheritdoc />
  public partial class create_db : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterDatabase()
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
         name: "Employees",
         columns: table => new
         {
           ID = table.Column<int>(type: "int", nullable: false)
                 .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
           FullName = table.Column<string>(type: "longtext", nullable: false)
                 .Annotation("MySql:CharSet", "utf8mb4"),
           Surname = table.Column<string>(type: "longtext", nullable: false)
                 .Annotation("MySql:CharSet", "utf8mb4"),
           Age = table.Column<string>(type: "longtext", nullable: false)
                 .Annotation("MySql:CharSet", "utf8mb4"),
           Number = table.Column<string>(type: "longtext", nullable: false)
                 .Annotation("MySql:CharSet", "utf8mb4"),
           DepartmentID = table.Column<int>(type: "int", nullable: false)
         },
         constraints: table =>
         {
           table.PrimaryKey("PK_Employees", x => x.ID);
         })
         .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "Departments",
          columns: table => new
          {
            ID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            DepartmentName = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ParentDepartmentID = table.Column<int>(type: "int", nullable: true),
            DepartmentImageLink = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Departments", x => x.ID);
          })
          .Annotation("MySql:CharSet", "utf8mb4");



      migrationBuilder.CreateTable(
          name: "IdentityRole<string>",
          columns: table => new
          {
            Id = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Name = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            NormalizedName = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Discriminator = table.Column<string>(type: "longtext", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_IdentityRole<string>", x => x.Id);
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "IdentityUser<string>",
          columns: table => new
          {
            Id = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            UserName = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Email = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
            PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
            TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
            LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
            LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
            AccessFailedCount = table.Column<int>(type: "int", nullable: false),
            Discriminator = table.Column<string>(type: "longtext", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Name = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_IdentityUser<string>", x => x.Id);
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "NumberOfEmployees",
          columns: table => new
          {
            DepartmentID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            EmployeeCount = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_NumberOfEmployees", x => x.DepartmentID);
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "RoleClaims",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            RoleId = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ClaimType = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_RoleClaims", x => x.Id);
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "UserClaims",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            UserId = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ClaimType = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserClaims", x => x.Id);
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "UserLogins",
          columns: table => new
          {
            LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            UserId = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "UserRoles",
          columns: table => new
          {
            UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.CreateTable(
          name: "UserTokens",
          columns: table => new
          {
            UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Name = table.Column<string>(type: "varchar(255)", nullable: false)
                  .Annotation("MySql:CharSet", "utf8mb4"),
            Value = table.Column<string>(type: "longtext", nullable: true)
                  .Annotation("MySql:CharSet", "utf8mb4")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
          })
          .Annotation("MySql:CharSet", "utf8mb4");

      migrationBuilder.InsertData(
          table: "Departments",
          columns: new[] { "ID", "DepartmentImageLink", "DepartmentName", "ParentDepartmentID" },
          values: new object[,]
          {
                    { 1, "/images/DepartmentImg/customer service department.jpg", "Отдел по обслуживанию клиентов", null },
                    { 2, "/images/DepartmentImg/production department.jpg", "Производственный отдел", null },
                    { 3, "/images/DepartmentImg/bookkeeping.jpg", "Бухгалтерия", null },
                    { 4, "/images/DepartmentImg/sales department.jpg", "Отдел продаж", 1 },
                    { 5, "/images/DepartmentImg/wholesale department.jpg", "Отдел оптовых продаж", 4 },
                    { 6, "/images/DepartmentImg/retail sales department.jpg", "Отдел розничных продаж", 4 },
                    { 7, "/images/DepartmentImg/logistics department.jpg", "Отдел логистики", 1 },
                    { 8, "/images/DepartmentImg/stock.jpg", "Склад", 7 },
                    { 9, "/images/DepartmentImg/bookkeeping.jpg", "Отдел доставки", 7 },
                    { 10, "/images/DepartmentImg/engineering department.jpg", "Инженерный отдел", 2 },
                    { 11, "/images/DepartmentImg/quality control department.jpg", "Отдел контроля качества", 2 },
                    { 12, "/images/DepartmentImg/purchasing department.jpg", "Отдел закупок", 2 }
          });

      migrationBuilder.InsertData(
          table: "Employees",
          columns: new[] { "ID", "Age", "DepartmentID", "FullName", "Number", "Surname" },
          values: new object[,]
          {
                    { 1, "28", 3, "Алексей", "+79123456789", "Иванов" },
                    { 2, "32", 3, "Екатерина", "+79123456780", "Смирнова" },
                    { 3, "21", 5, "Дмитрий", "+79123456781", "Козлов" },
                    { 4, "35", 5, "Анна", "+79123456782", "Петрова" },
                    { 5, "43", 6, "Сергей", "+79123456783", "Михайлов" },
                    { 6, "26", 6, "Ольга", "+79123456784", "Соколова" },
                    { 7, "29", 8, "Иван", "+79123456785", "Новиков" },
                    { 8, "31", 8, "Анастасия", "+79123456786", "Федорова" },
                    { 9, "40", 9, "Александр", "+79123456787", "Морозов" },
                    { 10, "27", 9, "Юлия", "+79123456788", "Волкова" },
                    { 11, "33", 9, "Михаил", "+79123456777", "Алексеев" },
                    { 12, "45", 9, "Елена", "+79123456776", "Лебедева" },
                    { 13, "39", 9, "Андрей", "+79123456775", "Семенов" },
                    { 14, "23", 10, "Мария", "+79123456774", "Егорова" },
                    { 15, "41", 10, "Владимир", "+79123456773", "Павлов" },
                    { 16, "30", 10, "Евгения", "+79123456772", "Ковалева" },
                    { 17, "37", 10, "Николай", "+79123456771", "Орлов" },
                    { 18, "34", 11, "Татьяна", "+79123456770", "Андреева" },
                    { 19, "42", 11, "Павел", "+79123456769", "Макаров" },
                    { 20, "22", 12, "Алиса", "+79123456768", "Николаева" }
          });

      migrationBuilder.InsertData(
          table: "IdentityRole<string>",
          columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
          values: new object[,]
          {
                    { "1", "ce97d922-c244-4fe4-a235-935a9f5b552a", "IdentityRole", "Admin", "ADMIN" },
                    { "2", "65dd32a3-73db-4315-a7ed-734a8507e406", "IdentityRole", "User", "USER" },
                    { "3", "92d4474a-4382-4bfc-95e1-070c242a4844", "IdentityRole", "Manager", "MANAGER" }
          });

      migrationBuilder.Sql(@"INSERT IGNORE INTO numberofemployees(DepartmentID, EmployeeCount)
                   WITH table_1 
                       AS (
                           WITH RECURSIVE cte
                               AS ( 
                                   SELECT d.ID, d.ID AS DepartmentID    
                                   FROM departments AS d   
                                   UNION   
                                   SELECT d.ID, cte.DepartmentID     
                                   FROM cte     
                                   JOIN departments AS d ON d.ParentDepartmentID = cte.ID
                               ) 	
                           SELECT cte.DepartmentID, COUNT(*) AS Employees  
                           FROM cte   
                           JOIN employees AS e ON e.DepartmentID = cte.ID   
                           GROUP BY cte.DepartmentID 
                       )
                   SELECT * FROM Table_1;");

      migrationBuilder.Sql(
          @"CREATE procedure UpdNumberOfEmployee()
                         BEGIN
                          UPDATE numberofemployees
                          INNER JOIN (
                           WITH RECURSIVE cte
                           AS ( 
                           SELECT d.ID, d.ID AS DepartmentID    
                           FROM departments AS d   
                           UNION   
                           SELECT d.ID, cte.DepartmentID     
                           FROM cte     
                           JOIN departments AS d ON d.ParentDepartmentID = cte.ID
                           ) 	
                           SELECT cte.DepartmentID, COUNT(*) AS Employees  
                           FROM cte   
                           JOIN employees AS e ON e.DepartmentID = cte.ID   
                           GROUP BY cte.DepartmentID
                          ) as Result USING(DepartmentID)
            SET numberofemployees.EmployeeCount = Result.Employees
            WHERE numberofemployees.DepartmentID = Result.DepartmentID 
            	AND numberofemployees.EmployeeCount <> Result.Employees;
                         END"
          );
      migrationBuilder.Sql(
          @"CREATE TRIGGER UpdNumberOfEmployee_Insert 
                         AFTER INSERT ON employees
                         FOR EACH ROW
                         BEGIN
                          CALL UpdNumberOfEmployee();
                         END"
          );

      migrationBuilder.Sql(
         @"CREATE TRIGGER UpdNumberOfEmployee_Delete
                         AFTER DELETE ON employees
                         FOR EACH ROW
                         BEGIN
                          CALL UpdNumberOfEmployee();
                         END"
         );

      migrationBuilder.Sql(
         @"CREATE TRIGGER UpdNumberOfEmployee_Update
                         AFTER UPDATE ON employees
                         FOR EACH ROW
                         BEGIN
                          CALL UpdNumberOfEmployee();
                         END"
         );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Departments");

      migrationBuilder.DropTable(
          name: "Employees");

      migrationBuilder.DropTable(
          name: "IdentityRole<string>");

      migrationBuilder.DropTable(
          name: "IdentityUser<string>");

      migrationBuilder.DropTable(
          name: "NumberOfEmployees");

      migrationBuilder.DropTable(
          name: "RoleClaims");

      migrationBuilder.DropTable(
          name: "UserClaims");

      migrationBuilder.DropTable(
          name: "UserLogins");

      migrationBuilder.DropTable(
          name: "UserRoles");

      migrationBuilder.DropTable(
          name: "UserTokens");
    }
  }
}
