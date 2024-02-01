using Microsoft.AspNetCore.Identity;

namespace Company.Models
{
  public class ApplicationUserModel : IdentityUser
  {
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Флаг первого входа в учетную запись
    /// </summary>
    public bool IsFirstLogin { get; set; }
  }
}
