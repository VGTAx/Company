using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models
{
    public class Department
    {
        public int? ID { get; set; }
        public string? DepartmentName { get; set; }        
        public int? ParentDepartmentID { get; set; }
        
        public int?  DepartmentDescriptionID { get; set; }
        [ForeignKey("DepartmentDescriptionID")]
        public DepartmentDescription ? DepartmentDescription { get; set; }
        public string ? DepartmentImageLink { get; set; }

        public Department(int ID, string? DepartmentName, int? ParentDepartmentID,
            int? DepartmentDescriptionID, string? DepartmentImageLink)
        {
            this.ID = ID;
            this.DepartmentName = DepartmentName;           
            this.ParentDepartmentID = ParentDepartmentID;
            this.DepartmentDescriptionID = DepartmentDescriptionID;
            this.DepartmentImageLink = DepartmentImageLink;
        }
        public Department() { }
    }
}
