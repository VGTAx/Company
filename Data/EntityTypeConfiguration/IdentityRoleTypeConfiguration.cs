using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class IdentityRoleTypeConfiguration : IEntityTypeConfiguration<IdentityRole>
  {
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
      builder.HasKey(ir => ir.Id);
      builder.HasData(
              new IdentityRole
              {
                Id = "1",
                Name = RoleClaims.Admin.ToString(),
                NormalizedName = RoleClaims.Admin.ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
              },
              new IdentityRole
              {
                Id = "2",
                Name = RoleClaims.User.ToString(),
                NormalizedName = RoleClaims.User.ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
              },
              new IdentityRole
              {
                Id = "3",
                Name = RoleClaims.Manager.ToString(),
                NormalizedName = RoleClaims.Manager.ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
              }
          );
    }
  }
}
