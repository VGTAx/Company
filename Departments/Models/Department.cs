namespace Company.Models
{
    public class Department
    {
        public int? ID { get; set; }
        public string? DepartmentName { get; set; }        
        public int? ParentDepartmentID { get; set; }  

        public Department(int ID, string? DepartmentName, int? ParentDepartmentID)
        {
            this.ID = ID;
            this.DepartmentName = DepartmentName;           
            this.ParentDepartmentID = ParentDepartmentID;                     
        }
        public Department() { }
    }
}
