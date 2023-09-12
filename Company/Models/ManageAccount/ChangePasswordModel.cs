using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
  /// <summary>
  /// Модель данных для изменения пароля пользователя.
  /// </summary>
  public class ChangePasswordModel
  {
    /// <summary>
    /// Старый пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите старый пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Старый пароль")]
    public string? OldPassword { get; set; }
    /// <summary>
    /// Новый пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите новый пароль")]
    [StringLength(100, ErrorMessage = "Длина пароля должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    public string? NewPassword { get; set; }
    /// <summary>
    /// Подтверждение пароля пользователя.
    /// </summary>
    [Required(ErrorMessage = "Введите подтверждение пароля")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Длина пароля должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("NewPassword", ErrorMessage = "Новый пароль и пароль подтверждения не совпадают.")]
    public string? ConfirmPassword { get; set; }
  }
}
