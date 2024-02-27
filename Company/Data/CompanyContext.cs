using Company.Data.EntityTypeConfiguration;
using Company.Interfaces;
using Company.Models;
using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Company.Data
{

  public class CompanyContext : IdentityDbContext<ApplicationUserModel>, ICompanyContext
  {
    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<DepartmentModel> Departments { get; set; }
    public DbSet<IdentityRole> IdentityRoles { get; set; }

    public CompanyContext(DbContextOptions<CompanyContext> options)
        : base(options) { }
    public CompanyContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new EmployeeTypeConfiguration());
      builder.ApplyConfiguration(new DepartmentTypeConfiguration());
      builder.ApplyConfiguration(new IdentityRoleTypeConfiguration());
      builder.ApplyConfiguration(new IdentityUserTypeConfiguration());
      builder.ApplyConfiguration(new IdentityUserClaimTypeConfiguration());

      builder.Entity<IdentityRoleClaim<string>>()
        .HasKey(d => d.Id);
      builder.Entity<IdentityUserLogin<string>>()
        .HasKey(d => new { d.LoginProvider, d.ProviderKey });
      builder.Entity<IdentityUserRole<string>>()
        .HasKey(d => new { d.UserId, d.RoleId });
      builder.Entity<IdentityUserToken<string>>()
        .HasKey(d => new { d.UserId, d.LoginProvider, d.Name });
    }

    public async Task<int> SaveChangesAsync()
    {
      return await base.SaveChangesAsync();
    }

    public new EntityEntry Update<TEntity>(TEntity entity)
    {
      return base.Update(entity!);
    }
  }
}