using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models
{
    public class DepartmentModel
    {
        public int? ID { get; set; }
        public string? DepartmentName { get; set; }
        public int? ParentDepartmentID { get; set; }

       
        public string? DepartmentImageLink { get; set; }

        public DepartmentModel(int ID, string? DepartmentName, int? ParentDepartmentID,
            string? DepartmentImageLink)
        {
            this.ID = ID;
            this.DepartmentName = DepartmentName;
            this.ParentDepartmentID = ParentDepartmentID;            
            this.DepartmentImageLink = DepartmentImageLink;
        }
        public DepartmentModel() { }
    }
}
