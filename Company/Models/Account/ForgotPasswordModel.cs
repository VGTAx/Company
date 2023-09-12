using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для запроса сброса пароля.
  /// </summary>
  public class ForgotPasswordModel
  {
    /// <summary>
    /// Адрес электронной почты пользователя, для которого запрашивается сброс пароля.
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
  }
}
