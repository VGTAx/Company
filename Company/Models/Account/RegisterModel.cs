using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для регистрации нового пользователя.
  /// </summary>
  public class RegisterModel
  {
    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите электронную почту")]
    [EmailAddress]
    [Display(Name = "Электроннная почта")]
    public string? Email { get; set; }
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите имя пользователя")]
    [Display(Name = "Имя")]
    public string? Name { get; set; }
    /// <summary>
    /// Пароль учетной записи пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, ErrorMessage = "{0} должен содержать от {2} до {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }
    /// <summary>
    /// Подтверждение пароля учетной записи пользователя.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Введенные пароли не совпадают")]
    public string? ConfirmPassword { get; set; }
  }
}
