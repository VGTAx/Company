using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Models.Employee
{
  /// <summary>
  /// Модель данных для сотрудника.
  /// </summary>
  public class EmployeeModel
  {
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    [HiddenInput]
    public int? ID { get; set; }

    /// <summary>
    /// Имя сотрудника.
    /// </summary>
    [Required(ErrorMessage = "Введите имя")]
    [Display(Name = "Имя")]
    public string? Name { get; set; }

    /// <summary>
    /// Фамилия сотрудника.
    /// </summary>
    [Required(ErrorMessage = "Введите фамилию")]
    [Display(Name = "Фамилия")]
    public string? Surname { get; set; }
    /// <summary>
    /// Возраст сотрудника.
    /// </summary>
    [Required(ErrorMessage = "Введите возраст")]
    [Display(Name = "Возраст")]
    public string? Age { get; set; }

    /// <summary>
    /// Номер телефона сотрудника.
    /// </summary>
    [Required(ErrorMessage = "Введите номер телефона")]
    [Display(Name = "Номер телефона")]
    public string? Number { get; set; }

    /// <summary>
    /// Идентификатор отдела сотрудника.
    /// </summary>
    [Required(ErrorMessage = "Введите отдел")]
    [Display(Name = "Отдел")]
    public int? DepartmentID { get; set; }

    /// <summary>
    /// Создает новый экземпляр класса <see cref="EmployeeModel"/>.
    /// </summary>
    /// <param name="ID">Идентификатор сотрудника.</param>
    /// <param name="DepartmentID">Идентификатор отдела.</param>
    public EmployeeModel(int? ID, int? DepartmentID)
    {
      this.ID = ID;
      this.DepartmentID = DepartmentID;
    }

    // <summary>
    /// Создает новый экземпляр класса <see cref="EmployeeModel"/>.
    /// </summary>
    /// <param name="iD">Идентификатор сотрудника.</param>
    /// <param name="name">Имя сотрудника.</param>
    /// <param name="surname">Фамилия сотрудника.</param>
    /// <param name="age">Возраст сотрудника.</param>
    /// <param name="number">Номер телефона сотрудника.</param>
    /// <param name="departmentID">Идентификатор отдела.</param>
    public EmployeeModel(int? iD, string name, string surname, string age, string number, int? departmentID)
    {
      ID = iD;
      Name = name;
      Surname = surname;
      Age = age;
      Number = number;
      DepartmentID = departmentID;
    }

    /// <summary>
    /// Создает новый экземпляр класса <see cref="EmployeeModel"/>.
    /// </summary>
    public EmployeeModel() { }
  }
}
