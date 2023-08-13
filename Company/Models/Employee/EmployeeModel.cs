using System.ComponentModel.DataAnnotations;

namespace Company_.Models.Employee
{
  public class EmployeeModel
  {    
    public int? ID { get; set; }
    [Required(ErrorMessage = "Введите имя")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Введите фамилию")]
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Введите возраст")]
    public string? Age { get; set; }
    [Required(ErrorMessage = "Введите номер телефона")]
    public string? Number { get; set; }
    [Required(ErrorMessage = "Введите отдел")]
    public int? DepartmentID { get; set; }

    public EmployeeModel(int? ID, int? DepartmentID)
    {
      this.ID = ID;
      this.DepartmentID = DepartmentID;
    }

    public EmployeeModel(int? iD, string name, string surname, string age, string number, int? departmentID)
    {
      ID = iD;
      Name = name;
      Surname = surname;
      Age = age;
      Number = number;
      DepartmentID = departmentID;
    }

    public EmployeeModel() { }
  }
}
