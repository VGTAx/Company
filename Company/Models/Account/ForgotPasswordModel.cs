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
    [Required(ErrorMessage = "Введите электронную почту")]
    [EmailAddress]
    [Display(Name = "Эл.почта")]
    public string? Email { get; set; }
  }
}
