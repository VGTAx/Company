using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

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
		                    SELECT DepartmentID, COUNT(*) AS Employees  
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
                name: "Departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(type: "longtext", nullable: true),
                    ParentDepartmentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Surname = table.Column<string>(type: "longtext", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<string>(type: "longtext", nullable: true),
                    DepartmentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NumberOfEmployees",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(type: "longtext", nullable: true),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfEmployees", x => x.DepartmentID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "ID", "DepartmentName", "ParentDepartmentID" },
                values: new object[,]
                {
                    { 1, "Отдел по обслуживанию клиентов", null },
                    { 2, "Производственный отдел", null },
                    { 3, "Бухгалтерия", null },
                    { 4, "Отдел продаж", 1 },
                    { 5, "Отдел оптовых продаж", 4 },
                    { 6, "Отдел розничных продаж", 4 },
                    { 7, "Отдел логистики", 1 },
                    { 8, "Склад", 7 },
                    { 9, "Отдел доставки", 7 },
                    { 10, "Инженерный отдел", 2 },
                    { 11, "Отдел проверки качества", 2 },
                    { 12, "Отдел закупок", 2 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Age", "DepartmentID", "Name", "Number", "Surname" },
                values: new object[,]
                {
                    { 1, 28, 3, "Алексей", "+79123456789", "Иванов" },
                    { 2, 32, 3, "Екатерина", "+79123456780", "Смирнова" },
                    { 3, 21, 5, "Дмитрий", "+79123456781", "Козлов" },
                    { 4, 35, 5, "Анна", "+79123456782", "Петрова" },
                    { 5, 43, 6, "Сергей", "+79123456783", "Михайлов" },
                    { 6, 26, 6, "Ольга", "+79123456784", "Соколова" },
                    { 7, 29, 8, "Иван", "+79123456785", "Новиков" },
                    { 8, 31, 8, "Анастасия", "+79123456786", "Федорова" },
                    { 9, 40, 9, "Александр", "+79123456787", "Морозов" },
                    { 10, 27, 9, "Юлия", "+79123456788", "Волкова" },
                    { 11, 33, 9, "Михаил", "+79123456777", "Алексеев" },
                    { 12, 45, 9, "Елена", "+79123456776", "Лебедева" },
                    { 13, 39, 9, "Андрей", "+79123456775", "Семенов" },
                    { 14, 23, 10, "Мария", "+79123456774", "Егорова" },
                    { 15, 41, 10, "Владимир", "+79123456773", "Павлов" },
                    { 16, 30, 10, "Евгения", "+79123456772", "Ковалева" },
                    { 17, 37, 10, "Николай", "+79123456771", "Орлов" },
                    { 18, 34, 11, "Татьяна", "+79123456770", "Андреева" },
                    { 19, 42, 11, "Павел", "+79123456769", "Макаров" },
                    { 20, 22, 12, "Алиса", "+79123456768", "Николаева" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "NumberOfEmployees");
        }
    }
}
