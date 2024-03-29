﻿using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
  /// <summary>
  /// Модель для входа пользователя в систему.
  /// </summary>
  public class LoginModel
  {
    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите электронную почту")]
    [EmailAddress]
    [Display(Name = "Эл.почта")]
    public string? Email { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    /// <summary>
    /// Флаг, указывающий, должна ли система запоминать пользователя.
    /// </summary>
    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
  }
}
