using Company.BaseClass;
using Company.Interfaces;
using Company.Models.Department;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Company.Services.Conrollers
{
  public class DepartmentService : DepartmentServiceBase<DepartmentModel>
  {
    private readonly ICompanyContext _context;

    public DepartmentService(ICompanyContext context)
    {
      _context = context;
    }

    public override IEnumerable<SelectListItem> GetDepartmentsListItem()
    {
      var departments = _context.Departments
         .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
         .Select(item => new SelectListItem
         {
           Value = item.ID.ToString(),
           Text = item.DepartmentName
         }).AsEnumerable();

      return departments;
    }

    public override async Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync()
    {
      return await _context.Departments.ToListAsync();
    }

    public override async Task<DepartmentModel> GetDepartmentAsync(int? id)
    {
      return id.HasValue ? await _context.Departments.FirstOrDefaultAsync(d => d.ID == id) : null;
    }
  }
}
