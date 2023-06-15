using Microsoft.EntityFrameworkCore;
using Company.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Company.Data
{
    public class DepartmentContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }       
        public DbSet<Employee> Employees { get; set; }
        public DbSet<NumberOfEmployee> NumberOfEmployees { get; set; }
        public DbSet<DepartmentDescription> DepartmentDescriptions { get; set; }
        

        public DepartmentContext(DbContextOptions<DepartmentContext> options) 
            : base(options) 
        {            
            Database.EnsureCreated();           
        }

        

        public DepartmentContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DepartmentNumberPoco>();

            modelBuilder.Entity<DepartmentDescription>().HasKey(d => d.DepartmentDescriptionID);
            modelBuilder.Entity<DepartmentDescription>().HasData(
                    new DepartmentDescription { DepartmentDescriptionID = 1,
                    Description = "Мы ценим наших клиентов и стремимся предоставить им высокий уровень обслуживания. " +
                    "Наш отдел по обслуживанию клиентов отвечает на вопросы, принимает заказы и разрешает любые " +
                    "возникающие проблемы, чтобы удовлетворить потребности наших клиентов. Отдел состоит из отдела продаж" +
                    "и отдела логистики"
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 2,
                        Description = "Наш производственный отдел отвечает за процесс производства. " +
                        "Мы используем передовые технологии и строгий контроль качества, чтобы обеспечить высокое " +
                        "качество нашей продукции. Отдел состоит из инженерного отдела, отдела проверки качества и" +
                        "отдела закупок"
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 3,
                        Description = "Наша бухгалтерия отвечает за финансовое управление и учет наших операций, включая" +
                        " учет расходов, доходов и подготовку финансовых отчетов."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 4,
                        Description = "Наш отдел продаж активно продвигает нашу продукцию на рынке. Мы работаем с " +
                        "оптовыми и розничными покупателями, устанавливая долгосрочные партнерские отношения и предлагая" +
                        " разнообразную продукцию."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 5,
                        Description = "Отдел, специализирующийся на обслуживании оптовых покупателей "+
                        "Мы предлагаем выгодные условия сотрудничества, широкий ассортимент продукции и " +
                        "помогаем нашим клиентам выбрать наиболее подходящую продукцию для их бизнеса.."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 6,
                        Description = "Наша компания имеет сеть розничных магазинов, где мы предлагаем нашу продукцию " +
                        "напрямую потребителям. Наш отдел розничных продаж работает на создание привлекательных витрин и" +
                        " предоставление высокого уровня обслуживания нашим клиентам."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 7,
                        Description = " Мы уделяем особое внимание эффективному управлению логистическими процессами." +
                        " Наш отдел логистики отвечает за координацию поставок, управление запасами, складирование и " +
                        "своевременную доставку наших продуктов."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 8,
                        Description = "У нас есть собственный склад, где мы храним наши товары в соответствии с " +
                        "высокими стандартами качества и безопасности."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 9,
                        Description = "Наш отдел доставки отвечает за оперативную и надежную доставку наших продуктов " +
                        "клиентам. Мы обеспечиваем, чтобы наша продуция достигала наших клиентов в сохранности " +
                        "и вовремя."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 10,
                        Description = " Наш инженерный отдел занимается разработкой и совершенствованием наших " +
                        "производственных процессов, а также внедрением новых технологий для повышения эффективности " +
                        "и качества нашей продукции."
                    },                   
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 11,
                        Description = "Мы придерживаемся строгих стандартов качества, и наш отдел проверки качества " +
                        "осуществляет тщательный контроль качества на всех этапах производства, чтобы гарантировать, " +
                        "что наши продукты отвечают высоким стандартам и требованиям клиентов."
                    },
                    new DepartmentDescription
                    {
                        DepartmentDescriptionID = 12,
                        Description = " Отдел закупок занимается поиском и приобретением качественных и надежных " +
                        "сырьевых материалов и компонентов для производства нашей продукции."
                    }
                    );
            modelBuilder.Entity<IdentityRole<string>>().HasKey(d => d.Id);
            modelBuilder.Entity<IdentityUser<string>>().HasKey(d => d.Id);
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(d => d.Id);
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(d => d.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(d => new { d.LoginProvider, d.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(d => new { d.UserId, d.RoleId }); 
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(d => new { d.UserId, d.LoginProvider, d.Name });
            
            modelBuilder.Entity<Department>().HasKey(d => d.ID);
           
            modelBuilder.Entity<Department>().HasData(
                 new Department(1, "Отдел по обслуживанию клиентов", null, 1, "/images/DepartmentImg/customer service department.jpg"),
                 new Department(2, "Производственный отдел", null, 2, "/images/DepartmentImg/production department.jpg"),
                 new Department(3, "Бухгалтерия", null, 3, "/images/DepartmentImg/bookkeeping.jpg"),
                 new Department(4, "Отдел продаж", 1, 4, "/images/DepartmentImg/sales department.jpg"),
                 new Department(5, "Отдел оптовых продаж", 4, 5, "/images/DepartmentImg/wholesale department.jpg"),
                 new Department(6, "Отдел розничных продаж", 4, 6, "/images/DepartmentImg/retail sales department.jpg"),
                 new Department(7, "Отдел логистики", 1, 7, "/images/DepartmentImg/logistics department.jpg"),
                 new Department(8, "Склад", 7, 8, "/images/DepartmentImg/stock.jpg"),
                 new Department(9, "Отдел доставки", 7, 9, "/images/DepartmentImg/bookkeeping.jpg"),
                 new Department(10, "Инженерный отдел", 2, 10, "/images/DepartmentImg/engineering department.jpg"),
                 new Department(11, "Отдел контроля качества", 2, 11, "/images/DepartmentImg/quality control department.jpg"),
                 new Department(12, "Отдел закупок", 2, 12, "/images/DepartmentImg/purchasing department.jpg")
                );      
            
            modelBuilder.Entity<Employee>()
                    .Property(e => e.ID)
                    .ValueGeneratedOnAdd()
                    .UseMySqlIdentityColumn();

            modelBuilder.Entity<Employee>().HasData(
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
