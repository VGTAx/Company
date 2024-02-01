using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Company.Interfaces
{
  public interface ICompanyContext : IDisposable
  {
    public DbSet<EmployeeModel> Employees { get; }
    public DbSet<DepartmentModel> Departments { get; }
    public DbSet<IdentityRole> IdentityRoles { get; }

    Task<int> SaveChangesAsync();
    EntityEntry Update<TEntity>(TEntity entity);
  }
}
