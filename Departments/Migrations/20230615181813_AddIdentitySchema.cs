using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"INSERT IGNORE INTO numberofemployees(DepartmentID, EmployeeCount)
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
               @"CREATE TRIGGER UpdNumberOfEmployee_Delete
                         AFTER UPDATE ON employees
                         FOR EACH ROW
                         BEGIN
                          CALL UpdNumberOfEmployee();
                         END"
               );

            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DepartmentDescriptions",
                columns: table => new
                {
                    DepartmentDescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentDescriptions", x => x.DepartmentDescriptionID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Surname = table.Column<string>(type: "longtext", nullable: false),
                    Age = table.Column<string>(type: "longtext", nullable: false),
                    Number = table.Column<string>(type: "longtext", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdentityRole<string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedName = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole<string>", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdentityUser<string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser<string>", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NumberOfEmployees",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfEmployees", x => x.DepartmentID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "longtext", nullable: true),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: true),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(type: "longtext", nullable: true),
                    ParentDepartmentID = table.Column<int>(type: "int", nullable: true),
                    DepartmentDescriptionID = table.Column<int>(type: "int", nullable: true),
                    DepartmentImageLink = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Departments_DepartmentDescriptions_DepartmentDescriptionID",
                        column: x => x.DepartmentDescriptionID,
                        principalTable: "DepartmentDescriptions",
                        principalColumn: "DepartmentDescriptionID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "DepartmentDescriptions",
                columns: new[] { "DepartmentDescriptionID", "Description" },
                values: new object[,]
                {
                    { 1, "Мы ценим наших клиентов и стремимся предоставить им высокий уровень обслуживания. Наш отдел по обслуживанию клиентов отвечает на вопросы, принимает заказы и разрешает любые возникающие проблемы, чтобы удовлетворить потребности наших клиентов. Отдел состоит из отдела продажи отдела логистики" },
                    { 2, "Наш производственный отдел отвечает за процесс производства. Мы используем передовые технологии и строгий контроль качества, чтобы обеспечить высокое качество нашей продукции. Отдел состоит из инженерного отдела, отдела проверки качества иотдела закупок" },
                    { 3, "Наша бухгалтерия отвечает за финансовое управление и учет наших операций, включая учет расходов, доходов и подготовку финансовых отчетов." },
                    { 4, "Наш отдел продаж активно продвигает нашу продукцию на рынке. Мы работаем с оптовыми и розничными покупателями, устанавливая долгосрочные партнерские отношения и предлагая разнообразную продукцию." },
                    { 5, "Отдел, специализирующийся на обслуживании оптовых покупателей Мы предлагаем выгодные условия сотрудничества, широкий ассортимент продукции и помогаем нашим клиентам выбрать наиболее подходящую продукцию для их бизнеса.." },
                    { 6, "Наша компания имеет сеть розничных магазинов, где мы предлагаем нашу продукцию напрямую потребителям. Наш отдел розничных продаж работает на создание привлекательных витрин и предоставление высокого уровня обслуживания нашим клиентам." },
                    { 7, " Мы уделяем особое внимание эффективному управлению логистическими процессами. Наш отдел логистики отвечает за координацию поставок, управление запасами, складирование и своевременную доставку наших продуктов." },
                    { 8, "У нас есть собственный склад, где мы храним наши товары в соответствии с высокими стандартами качества и безопасности." },
                    { 9, "Наш отдел доставки отвечает за оперативную и надежную доставку наших продуктов клиентам. Мы обеспечиваем, чтобы наша продуция достигала наших клиентов в сохранности и вовремя." },
                    { 10, " Наш инженерный отдел занимается разработкой и совершенствованием наших производственных процессов, а также внедрением новых технологий для повышения эффективности и качества нашей продукции." },
                    { 11, "Мы придерживаемся строгих стандартов качества, и наш отдел проверки качества осуществляет тщательный контроль качества на всех этапах производства, чтобы гарантировать, что наши продукты отвечают высоким стандартам и требованиям клиентов." },
                    { 12, " Отдел закупок занимается поиском и приобретением качественных и надежных сырьевых материалов и компонентов для производства нашей продукции." }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Age", "DepartmentID", "Name", "Number", "Surname" },
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
                table: "Departments",
                columns: new[] { "ID", "DepartmentDescriptionID", "DepartmentImageLink", "DepartmentName", "ParentDepartmentID" },
                values: new object[,]
                {
                    { 1, 1, "/images/DepartmentImg/customer service department.jpg", "Отдел по обслуживанию клиентов", null },
                    { 2, 2, "/images/DepartmentImg/production department.jpg", "Производственный отдел", null },
                    { 3, 3, "/images/DepartmentImg/bookkeeping.jpg", "Бухгалтерия", null },
                    { 4, 4, "/images/DepartmentImg/sales department.jpg", "Отдел продаж", 1 },
                    { 5, 5, "/images/DepartmentImg/wholesale department.jpg", "Отдел оптовых продаж", 4 },
                    { 6, 6, "/images/DepartmentImg/retail sales department.jpg", "Отдел розничных продаж", 4 },
                    { 7, 7, "/images/DepartmentImg/logistics department.jpg", "Отдел логистики", 1 },
                    { 8, 8, "/images/DepartmentImg/stock.jpg", "Склад", 7 },
                    { 9, 9, "/images/DepartmentImg/bookkeeping.jpg", "Отдел доставки", 7 },
                    { 10, 10, "/images/DepartmentImg/engineering department.jpg", "Инженерный отдел", 2 },
                    { 11, 11, "/images/DepartmentImg/quality control department.jpg", "Отдел контроля качества", 2 },
                    { 12, 12, "/images/DepartmentImg/purchasing department.jpg", "Отдел закупок", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentDescriptionID",
                table: "Departments",
                column: "DepartmentDescriptionID");
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

            migrationBuilder.DropTable(
                name: "DepartmentDescriptions");
        }
    }
}
