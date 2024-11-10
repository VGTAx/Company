using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class IdentityUserClaimTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
  {
    private readonly string _id;
    public IdentityUserClaimTypeConfiguration(string id)
    {
      _id = id;
    }

    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
      builder.HasKey(iuc => iuc.Id);
      builder.HasData(
          new IdentityUserClaim<string>
          {
            Id = 5,
            ClaimType = ClaimTypes.Role,
            ClaimValue = RoleClaims.Admin.ToString(),
            UserId = _id,
          },
          new IdentityUserClaim<string>
          {
            Id = 6,
            ClaimType = ClaimTypes.Role,
            ClaimValue = RoleClaims.User.ToString(),
            UserId = _id,
          }
        );
    }
  }
}
