using System.ComponentModel.DataAnnotations;

namespace Company.Models.Employee
{
  public class EmployeeModel
  {
    public int? ID { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public string? Age { get; set; }
    [Required]
    public string? Number { get; set; }
    [Required]
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
