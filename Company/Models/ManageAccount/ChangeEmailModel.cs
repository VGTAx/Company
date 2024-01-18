using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
  /// <summary>
  /// Модель для изменения адреса электронной почты.
  /// </summary>
  public class ChangeEmailModel
  {
    /// <summary>
    /// Текущий адрес электронной почты.
    /// </summary>
    [EmailAddress]
    [Display(Name = "Эл.почта")]
    [Required(ErrorMessage = "Введите эл.почту")]
    public string? Email { get; set; }

    /// <summary>
    /// Новый адрес электронной почты.
    /// </summary>
    [EmailAddress]
    [Display(Name = "Новая эл.почта")]
    [Required(ErrorMessage = "Введите эл.почту")]
    public string? NewEmail { get; set; }

    /// <summary>
    /// Флаг, указывающий, подтвержден ли новый адрес электронной почты.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }
  }
}
