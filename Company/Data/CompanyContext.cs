using Company.Models;
using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Data
{  
  public class CompanyContext : IdentityDbContext<ApplicationUserModel>
  {    
    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<DepartmentModel> Departments { get; set; }
    public DbSet<IdentityRole> IdentityRoles { get; set; }
    
    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options)   { }

    public CompanyContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
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

      builder.Entity<EmployeeModel>()
             .Property(e => e.ID)
             .ValueGeneratedOnAdd()
             .UseMySqlIdentityColumn();

      //builder.Entity<EmployeeModel>().HasData(
      //    new EmployeeModel(1, "Алексей", "Иванов", "28", "+79123456789", 3),
      //    new EmployeeModel(2, "Екатерина", "Смирнова", "32", "+79123456780", 3),
      //    new EmployeeModel(3, "Дмитрий", "Козлов", "21", "+79123456781", 5),
      //    new EmployeeModel(4, "Анна", "Петрова", "35", "+79123456782", 5),
      //    new EmployeeModel(5, "Сергей", "Михайлов", "43", "+79123456783", 6),
      //    new EmployeeModel(6, "Ольга", "Соколова", "26", "+79123456784", 6),
      //    new EmployeeModel(7, "Иван", "Новиков", "29", "+79123456785", 8),
      //    new EmployeeModel(8, "Анастасия", "Федорова", "31", "+79123456786", 8),
      //    new EmployeeModel(9, "Александр", "Морозов", "40", "+79123456787", 9),
      //    new EmployeeModel(10, "Юлия", "Волкова", "27", "+79123456788", 9),
      //    new EmployeeModel(11, "Михаил", "Алексеев", "33", "+79123456777", 9),
      //    new EmployeeModel(12, "Елена", "Лебедева", "45", "+79123456776", 9),
      //    new EmployeeModel(13, "Андрей", "Семенов", "39", "+79123456775", 9),
      //    new EmployeeModel(14, "Мария", "Егорова", "23", "+79123456774", 10),
      //    new EmployeeModel(15, "Владимир", "Павлов", "41", "+79123456773", 10),
      //    new EmployeeModel(16, "Евгения", "Ковалева", "30", "+79123456772", 10),
      //    new EmployeeModel(17, "Николай", "Орлов", "37", "+79123456771", 10),
      //    new EmployeeModel(18, "Татьяна", "Андреева", "34", "+79123456770", 11),
      //    new EmployeeModel(19, "Павел", "Макаров", "42", "+79123456769", 11),
      //    new EmployeeModel(20, "Алиса", "Николаева", "22", "+79123456768", 12)
      //    );

      builder.Entity<DepartmentModel>().HasKey(d => d.ID);

      builder.Entity<DepartmentModel>().HasData(
           new DepartmentModel(1, "Отдел по обслуживанию клиентов", null),
           new DepartmentModel(2, "Производственный отдел", null),
           new DepartmentModel(3, "Бухгалтерия", null),
           new DepartmentModel(4, "Отдел продаж", 1),
           new DepartmentModel(5, "Отдел оптовых продаж", 4),
           new DepartmentModel(6, "Отдел розничных продаж", 4),
           new DepartmentModel(7, "Отдел логистики", 1),
           new DepartmentModel(8, "Склад", 7),
           new DepartmentModel(9, "Отдел доставки", 7),
           new DepartmentModel(10, "Инженерный отдел", 2),
           new DepartmentModel(11, "Отдел контроля качества", 2),
           new DepartmentModel(12, "Отдел закупок", 2)
          );
    }
  }
}