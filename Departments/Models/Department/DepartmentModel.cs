namespace Company.Models.Department
{
  public class DepartmentModel
  {
    public int? ID { get; set; }
    public string? DepartmentName { get; set; }
    public int? ParentDepartmentID { get; set; }

    public DepartmentModel(int? id, string? departmentName, int? parentDepartmentID)
    {
      ID = id;
      DepartmentName = departmentName;
      ParentDepartmentID = parentDepartmentID;
    }
    public DepartmentModel() { }
  }
}
