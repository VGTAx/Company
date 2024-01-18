using System.ComponentModel.DataAnnotations;

namespace Company.Models.Department
{
  /// <summary>
  /// Модель данных для хранения числа сотрудников в отделе.
  /// </summary>
  public class NumberOfEmployee
  {
    /// <summary>
    /// Идентификатор отдела.
    /// </summary>
    [Key]
    public int? DepartmentID { get; set; }
    /// <summary>
    /// Количество сотрудников в отделе.
    /// </summary>
    [Display(Name = "Количество сотрудников")]
    public int EmployeeCount { get; set; }
  }
}
