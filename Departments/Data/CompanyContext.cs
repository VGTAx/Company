using Microsoft.EntityFrameworkCore;
using Company.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Company.Data
{
    public class CompanyContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }       
        public DbSet<Employee> Employees { get; set; }
        public DbSet<NumberOfEmployee> NumberOfEmployees { get; set; }        
        public DbSet<IdentityRole> IdentityRoles { get; set; }

        public CompanyContext(DbContextOptions<CompanyContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }       

        public CompanyContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<DepartmentNumberPoco>();
            
            builder.Entity<IdentityRole<string>>().HasKey(d => d.Id);
            builder.Entity<IdentityUser<string>>().HasKey(d => d.Id);
            builder.Entity<IdentityRoleClaim<string>>().HasKey(d => d.Id);
            builder.Entity<IdentityUserClaim<string>>().HasKey(d => d.Id);
            builder.Entity<IdentityUserLogin<string>>().HasKey(d => new { d.LoginProvider, d.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(d => new { d.UserId, d.RoleId }); 
            builder.Entity<IdentityUserToken<string>>().HasKey(d => new { d.UserId, d.LoginProvider, d.Name });

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "1",
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),                       
                    },
                    new IdentityRole
                    {
                        Id = "2",
                        Name = "User",
                        NormalizedName = "USER",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    },
                    new IdentityRole
                    {
                        Id = "3",
                        Name = "Manager",
                        NormalizedName = "MANAGER",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    }
                );

            builder.Entity<Department>().HasKey(d => d.ID);
           
            builder.Entity<Department>().HasData(
                 new Department(1, "Отдел по обслуживанию клиентов", null, "/images/DepartmentImg/customer service department.jpg"),
                 new Department(2, "Производственный отдел", null, "/images/DepartmentImg/production department.jpg"),
                 new Department(3, "Бухгалтерия", null, "/images/DepartmentImg/bookkeeping.jpg"),
                 new Department(4, "Отдел продаж", 1, "/images/DepartmentImg/sales department.jpg"),
                 new Department(5, "Отдел оптовых продаж", 4, "/images/DepartmentImg/wholesale department.jpg"),
                 new Department(6, "Отдел розничных продаж", 4, "/images/DepartmentImg/retail sales department.jpg"),
                 new Department(7, "Отдел логистики", 1, "/images/DepartmentImg/logistics department.jpg"),
                 new Department(8, "Склад", 7, "/images/DepartmentImg/stock.jpg"),
                 new Department(9, "Отдел доставки", 7, "/images/DepartmentImg/bookkeeping.jpg"),
                 new Department(10, "Инженерный отдел", 2, "/images/DepartmentImg/engineering department.jpg"),
                 new Department(11, "Отдел контроля качества", 2, "/images/DepartmentImg/quality control department.jpg"),
                 new Department(12, "Отдел закупок", 2, "/images/DepartmentImg/purchasing department.jpg")
                );      
            
            builder.Entity<Employee>()
                    .Property(e => e.ID)
                    .ValueGeneratedOnAdd()
                    .UseMySqlIdentityColumn();

            builder.Entity<Employee>().HasData(
                new Employee(1, "Алексей", "Иванов", "28", "+79123456789", 3), 
                new Employee(2, "Екатерина", "Смирнова", "32", "+79123456780", 3),
                new Employee(3, "Дмитрий", "Козлов", "21", "+79123456781", 5), 
                new Employee(4, "Анна", "Петрова", "35", "+79123456782", 5),
                new Employee(5, "Сергей", "Михайлов", "43", "+79123456783", 6),
                new Employee(6, "Ольга", "Соколова", "26", "+79123456784", 6),
                new Employee(7, "Иван", "Новиков", "29", "+79123456785", 8), 
                new Employee(8, "Анастасия", "Федорова", "31", "+79123456786", 8),
                new Employee(9, "Александр", "Морозов", "40", "+79123456787", 9), 
                new Employee(10, "Юлия", "Волкова", "27", "+79123456788", 9), 
                new Employee(11, "Михаил", "Алексеев", "33", "+79123456777", 9), 
                new Employee(12, "Елена", "Лебедева", "45", "+79123456776", 9), 
                new Employee(13, "Андрей", "Семенов", "39", "+79123456775", 9),
                new Employee(14, "Мария", "Егорова", "23", "+79123456774", 10), 
                new Employee(15, "Владимир", "Павлов", "41", "+79123456773", 10), 
                new Employee(16, "Евгения", "Ковалева", "30", "+79123456772", 10), 
                new Employee(17, "Николай", "Орлов", "37", "+79123456771", 10),
                new Employee(18, "Татьяна", "Андреева", "34", "+79123456770", 11), 
                new Employee(19, "Павел", "Макаров", "42", "+79123456769", 11),
                new Employee(20, "Алиса", "Николаева", "22", "+79123456768", 12)
                );

        }
    }

        
}
