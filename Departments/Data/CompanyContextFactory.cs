//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace Company.Data
//{
//    public class CompanyContextFactory : IDesignTimeDbContextFactory<CompanyContext>
//    {
//        public CompanyContext CreateDbContext(string[] args)
//        {
//            var connectionString = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json")
//                .Build()
//                .GetConnectionString("MySqlConnection");

//            var optionsBuilder = new DbContextOptionsBuilder<CompanyContext>();
//            optionsBuilder.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString));

//            return new CompanyContext(optionsBuilder.Options);
//        }
//    }
//}
