using System.ComponentModel.DataAnnotations;

namespace Company.Models.Department
{
  /// <summary>
  /// Модель информации об отделе.
  /// </summary>
  public class DepartmentModel
  {
    /// <summary>
    /// Идентификатор отдела.
    /// </summary>
    public int? ID { get; set; }
    /// <summary>
    /// Название отдела.
    /// </summary>
    [Display(Name = "Отдел")]
    public string? DepartmentName { get; set; }
    /// <summary>
    /// Идентификатор родительского отдела.
    /// </summary>
    public int? ParentDepartmentID { get; set; }

    /// <summary>
    /// Создает экземпляр класса <see cref="DepartmentModel"/>.
    /// </summary>
    /// <param name="id">Идентификатор отдела</param>
    /// <param name="departmentName">Название отдела</param>
    /// <param name="parentDepartmentID">Идентификатор родительского отдела</param>
    public DepartmentModel(int? id, string? departmentName, int? parentDepartmentID)
    {
      ID = id;
      DepartmentName = departmentName;
      ParentDepartmentID = parentDepartmentID;
    }
    /// <summary>
    /// Создает экземпляр класса <see cref="DepartmentModel"/>.
    /// </summary>
    public DepartmentModel() { }
  }
}
