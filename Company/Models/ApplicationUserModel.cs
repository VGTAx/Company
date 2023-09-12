using Microsoft.AspNetCore.Identity;

namespace Company.Models
{
  public class ApplicationUserModel : IdentityUser
  {
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? Name { get; set; }
  }
}
