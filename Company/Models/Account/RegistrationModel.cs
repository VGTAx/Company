using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для регистрации нового пользователя.
  /// </summary>
  public class RegistrationModel
  {
    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    [Remote(action: "CheckExistEmail", controller: "Account", ErrorMessage = "Test")]
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
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{6,}$", ErrorMessage = "Пароль должен содержать как минимум одну цифру, " +
    @"одну прописную букву, одну заглавную букву и один специальный символ, и быть длиной не менее 6 символов.")]
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
