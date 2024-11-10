using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class IdentityUserTypeConfiguration : IEntityTypeConfiguration<AppUser>
  {
    private readonly string _id;

    public IdentityUserTypeConfiguration(string id)
    {
      _id = id;
    }

    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
      builder.HasKey(iu => iu.Id);
      builder.HasData(
        new AppUser
        {
          Id = _id,
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
