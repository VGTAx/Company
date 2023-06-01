using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Employee
    {
        public int ?ID { get; set; }
        [Required]
        public string ?Name { get; set; }
        [Required]
        public string ?Surname { get; set; }
        [Required]
        public int ?Age { get; set; }
        [Required]
        public string ?Number { get; set; }
        [Required]
        public int ?DepartmentID { get; set; }

        public Employee(int? ID, int? DepartmentID)
        {
            this.ID = ID;
            this.DepartmentID = DepartmentID;
        }       

        public Employee(int? iD, string name, string surname, int age, string number, int? departmentID)
        {
            ID = iD;
            Name = name;
            Surname = surname;
            Age = age;
            Number = number;
            DepartmentID = departmentID;
        }

        public Employee() { }
    }
}
