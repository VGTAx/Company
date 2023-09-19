using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для сброса пароля пользователя.
  /// </summary>
  public class ResetPasswordModel
  {
    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите электронную почту")]    
    public string? Email { get; set; }
    /// <summary>
    /// Новый пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, ErrorMessage = "{0} должен содержать от {2} до {1} символов.", MinimumLength = 6)]
    [Display(Name = "Новый пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    /// <summary>
    /// Подтверждение нового пароля пользователя.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Введенные пароли не совпадают")]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Код для сброса пароля.
    /// </summary>
    [Required]
    public string? Code { get; set; }
  }
}
