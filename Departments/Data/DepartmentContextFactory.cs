using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore;

namespace Company.Data
{
    public class DepartmentContextFactory : IDesignTimeDbContextFactory<DepartmentContext>
    {
        public DepartmentContext CreateDbContext(string[] args)
        {            
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("MySqlConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DepartmentContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new DepartmentContext(optionsBuilder.Options);
        }
    }
}
