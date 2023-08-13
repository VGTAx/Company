using Company_.Data;
using Company_.Models.Department;
using Company_.Models.Employee;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Company_.Controllers
{
  
  [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class EmployeeController : Controller
  {
    private readonly CompanyContext _context;

    public EmployeeController(CompanyContext context)
    {
      _context = context;
    }

    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Create(int? departmentId)
    {
      var departments = _context.Departments
          .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
          .Select(item => new SelectListItem
          {
            Value = item.ID.ToString(),
            Text = item.DepartmentName
          }).AsEnumerable();

      if (departmentId != null)
      {
        var tempDep = GetDepartments(departmentId, _context.Departments.ToList());
        departments = tempDep.Select(item => new SelectListItem 
            {
              Value = item.ID.ToString(),
              Text = item.DepartmentName,
              Selected = (tempDep.Count == 1)
            });
      }

      ViewData["Departments"] = departments;
      return View();
    }
		
		[HttpPost]    
    public async Task<IActionResult> Create([Bind("ID, Name, Surname, Age, Number, DepartmentID")] EmployeeModel employee)
    {
      if (ModelState.IsValid)
      {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details");
      }

      var departments = _context.Departments
          .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
          .Select(item => new SelectListItem
          {
            Value = item.ID.ToString(),
            Text = item.DepartmentName
          }).AsEnumerable();

      ViewData["Departments"] = departments;

      return View("Create");
    }

    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Employees == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);

      if (employee == null)
      {
        return NotFound();
      }

      var departments = _context.Departments
          .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
          .Select(item => new SelectListItem
          {
            Value = item.ID.ToString(),
            Text = item.DepartmentName
          });

      ViewData["Departments"] = departments;

      return View("Edit", employee);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(int? id, [Bind("ID, Name, Surname, Age, Number, DepartmentID")] EmployeeModel employee)
    {
      if (id != employee.ID)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(employee);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!EmployeeExists(employee.ID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Details));
      }

      var departments = _context.Departments
          .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
          .Select(item => new SelectListItem
          {
            Value = item.ID.ToString(),
            Text = item.DepartmentName
          }).AsEnumerable();

      ViewData["Departments"] = departments;

      return View();
    }

    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Employees == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);

      if (employee == null)
      {
        return NotFound();
      }

      var department = await _context.Departments
          .FirstOrDefaultAsync(d => d.ID == employee.DepartmentID);

      ViewBag.Department = department!.DepartmentName;

      return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.Employees == null)
      {
        return Problem("Entity set 'DepartmentContext.Employee' is null.");
      }

      var employee = _context.Employees.FirstOrDefault(e => e.ID == id);

      if (employee != null)
      {
        _context.Employees.Remove(employee);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction("Details");
    }

    [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]   
    public IActionResult Details()
    {
      var departments = _context.Departments.ToList();
      var employee = _context.Employees;

      ViewBag.Departments = departments;

      return View(employee);
    }

    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    private List<DepartmentModel> GetDepartments(int? id, List<DepartmentModel> departments)
    {
      var subdepartments = departments.Where(d => d.ParentDepartmentID == id).ToList();
      var deps = new List<DepartmentModel>();

      if (subdepartments.Any())
      {
        foreach (var department in subdepartments)
        {
          var childDep = GetDepartments(department.ID, departments);
          if (childDep.Count == 0)
          {
            deps.Add(department);
          }
          else
          {
            deps.AddRange(childDep);
          }
        }
      }
      else
      {
        deps.Add(departments.FirstOrDefault(d => d.ID == id)!);
      }
      return deps;
    }

    private bool EmployeeExists(int? id)
    {
      return _context.Employees.Any(e => e.ID == id);
    }

    public IActionResult Index()
    {
      return View();
    }
  }
}
