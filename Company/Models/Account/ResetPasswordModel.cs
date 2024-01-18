using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для сброса пароля пользователя.
  /// </summary>
  public class ResetPasswordModel
  {
    /// <summary>
    /// Код для сброса пароля.
    /// </summary>
    [Required]
    [HiddenInput]
    public string? Code { get; set; }

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    [EmailAddress]
    [Required]
    [HiddenInput]
    public string? Email { get; set; }

    /// <summary>
    /// Новый пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите пароль")]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{6,}$", ErrorMessage = "Пароль должен содержать как минимум одну цифру, " +
    @"одну прописную букву, одну заглавную букву и один специальный символ, и быть длиной не менее 6 символов.")]
    [Display(Name = "Новый пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    /// <summary>
    /// Подтверждение нового пароля пользователя.
    /// </summary>
    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Введенные пароли не совпадают")]
    public string? ConfirmPassword { get; set; }
  }
}
