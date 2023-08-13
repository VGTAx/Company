using Microsoft.AspNetCore.Identity;

namespace Company_.Models
{
  public class ApplicationUserModel : IdentityUser
  {
    public string? Name { get; set; }
  }
}
