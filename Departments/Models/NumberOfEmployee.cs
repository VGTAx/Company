using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class NumberOfEmployee
    {
        [Key]
        public int ?DepartmentID { get; set; }        
        public int EmployeeCount { get; set; }        
    }
}
