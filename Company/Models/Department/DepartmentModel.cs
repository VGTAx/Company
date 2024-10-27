using System.ComponentModel.DataAnnotations;

namespace Company.Models.Departments
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
    public DepartmentModel(Department? department, string? departmentName, Department? parentDepartment)
    {
      ID = ((int?)department);
      DepartmentName = departmentName;
      ParentDepartmentID = ((int?)parentDepartment);
    }
    /// <summary>
    /// Создает экземпляр класса <see cref="DepartmentModel"/>.
    /// </summary>
    public DepartmentModel() { }
  }
}

public enum Department
{
  CustomerService = 1,
  Production,
  Accounting,
  Sales,
  Wholesales,
  RetailSales,
  Logistic,
  Warehouse,
  Delivering,
  Engineering,
  QualityControl,
  Purchasing
}