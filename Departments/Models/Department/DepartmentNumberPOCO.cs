namespace Company.Models.Department
{
    public class DepartmentNumberPoco
    {
        public int? ID { get; set; }
        public string? DepartmentName { get; set; }
        public int? ParentID { get; set; }
        public int? NumberEmployee { get; set; }

        public DepartmentNumberPoco(int? ID, string? departmentName, int? parentID, int? numberEmployee)
        {
            this.ID = ID;
            DepartmentName = departmentName;
            ParentID = parentID;
            NumberEmployee = numberEmployee;
        }
    }
}
