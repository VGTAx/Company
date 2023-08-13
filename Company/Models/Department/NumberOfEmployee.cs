using System.ComponentModel.DataAnnotations;

namespace Company_.Models.Department
{
  public class NumberOfEmployee
  {
    [Key]
    public int? DepartmentID { get; set; }
    public int EmployeeCount { get; set; }
  }
}
