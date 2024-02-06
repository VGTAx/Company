using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class IdentityUserTypeConfiguration : IEntityTypeConfiguration<ApplicationUserModel>
  {
    public void Configure(EntityTypeBuilder<ApplicationUserModel> builder)
    {
      builder.HasKey(iu => iu.Id);
      builder.HasData(
        new ApplicationUserModel
        {
          Id = "aef4d787-6c8a-4f3d-bd2e-9b77f82bdc1a",
          Name = "Admin",
          UserName = "putinvodkagta@yandex.by",
          NormalizedUserName = "PUTINVODKAGTA@YANDEX.BY",
          Email = "putinvodkagta@yandex.by",
          NormalizedEmail = "PUTINVODKAGTA@YANDEX.BY",
          EmailConfirmed = true,
          PasswordHash = new PasswordHasher<string>().HashPassword("", "password1!"),
          ConcurrencyStamp = Guid.NewGuid().ToString(),
          SecurityStamp = Guid.NewGuid().ToString(),
          LockoutEnabled = true,
          IsFirstLogin = true
        });
    }
  }
}
