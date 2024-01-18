using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
  /// <summary>
  /// Модель данных профиля пользователя.
  /// </summary>
  public class ProfileModel
  {
    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Эл.почта")]
    public string? Email { get; set; }

    /// <summary>
    /// Номер телефона пользователя.
    /// </summary>
    [Display(Name = "Номер телефона")]
    [Phone(ErrorMessage = "Введен некорректный номер телефона.")]
    public string? Phone { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Display(Name = "Имя")]
    public string? Name { get; set; }
  }
}
