using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class IdentityUserClaimTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
  {
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
      builder.HasKey(iuc => iuc.Id);
      builder.HasData(
          new IdentityUserClaim<string>
          {
            Id = 5,
            ClaimType = ClaimTypes.Role,
            ClaimValue = "Admin",
            UserId = "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a",
          },
          new IdentityUserClaim<string>
          {
            Id = 6,
            ClaimType = ClaimTypes.Role,
            ClaimValue = "User",
            UserId = "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a",
          }
        );
    }
  }
}
