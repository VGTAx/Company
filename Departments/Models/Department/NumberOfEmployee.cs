using System.ComponentModel.DataAnnotations;

namespace Company.Models.Department
{
    public class NumberOfEmployee
    {
        [Key]
        public int? DepartmentID { get; set; }
        public int EmployeeCount { get; set; }
    }
}
