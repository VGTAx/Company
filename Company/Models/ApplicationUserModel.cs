using Microsoft.AspNetCore.Identity;

namespace Company.Models
{
  public class ApplicationUserModel : IdentityUser
  {
    public string? Name { get; set; }
  }
}
