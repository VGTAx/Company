using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
  /// <summary>
  /// Модель для удаления учетной записи пользователя
  /// </summary>
  public class DeletePersonalDataModel
  {
    /// <summary>
    /// Пароль для подтверждения удаления учетной записи
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [Required]
    public string? Password { get; set; }

    /// <summary>
    /// Флаг, указывающий, требуется ли ввод пароля для удаления учетной записи.
    /// </summary>
    public bool RequirePassword { get; set; }
  }
}
