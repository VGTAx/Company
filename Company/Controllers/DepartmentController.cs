using Company.Data;
using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер для управления отделами.
  /// </summary>
  public class DepartmentController : Controller
  {
    private readonly CompanyContext _context;
    /// <summary>
    /// Инициализирует новый экземпляр контроллера для управления отделами с использованием указанного контекста компании.
    /// </summary>
    /// <param name="context">Контекст компании для доступа к данным отделов.</param>
    public DepartmentController(CompanyContext context)
    {
      _context = context;
    }
    /// <summary>
    /// Отображает список отделов и информацию о количестве сотрудников в каждом отделе.
    /// </summary>
    /// <returns>View с данными об отделах и количестве сотрудников.</returns>
    public async Task<IActionResult> Index()
    {
      var numberOfEmployyes = from e in _context.Departments
                              select new DepartmentModel(e.ID, e.DepartmentName, e.ParentDepartmentID);

      return View(await numberOfEmployyes.ToListAsync());
    }
    /// <summary>
    /// Отображает информацию о выбранном отделе.
    /// </summary>
    /// <param name="departmentId">Идентификатор отдела.</param>
    /// <param name="departmentName">Название отдела.</param>
    /// <returns>View с информацией о выбранном отделе.</returns>
    public IActionResult Details(int? departmentId = 0, string? departmentName = null)
    {
      if (departmentName == null && departmentId == null)
      {
        return NotFound();
      }
      else if (departmentId == 0 && !String.IsNullOrEmpty(departmentName))
      {
        departmentId = _context.Departments.FirstOrDefault(d => d.DepartmentName == departmentName)!.ID;
      }

      var department = _context.Departments.FirstOrDefault(d => d.ID == departmentId);

      switch (departmentId)
      {
        case 1:
          return View("CustomerService", department);
        case 2:
          return View("Production", department);
        case 3:
          return View("Bookkeeping", department);
        case 4:
          return View("Sales", department);
        case 5:
          return View("WholeSales", department);
        case 6:
          return View("RetailSales", department);
        case 7:
          return View("Logistic", department);
        case 8:
          return View("Stock", department);
        case 9:
          return View("Delivery", department);
        case 10:
          return View("Engineering", department);
        case 11:
          return View("QualityControl", department);
        case 12:
          return View("Purchasing", department);
        default:
          return NotFound();
      }
    }
    /// <summary>
    /// Получает список сотрудников для указанного отдела.
    /// </summary>
    /// <param name="id">Идентификатор отдела.</param>
    /// <param name="departments">Список отделов.</param>
    /// <param name="empl">Список сотрудников.</param>
    /// <returns>Список сотрудников для указанного отдела.</returns>
    private List<EmployeeModel> GetEmployees(int? id, List<DepartmentModel> departments, List<EmployeeModel> empl)
    {
      var subdepartments = departments.Where(d => d.ParentDepartmentID == id).ToList();
      var employees = new List<EmployeeModel>();

      if (subdepartments.Count != 0)
      {
        employees = empl.Where(e => subdepartments.Select(s => s.ID).Contains(e.ID)).ToList();
      }
      employees = empl.Where(e => e.DepartmentID == id).ToList();

      if (subdepartments.Any())
      {
        foreach (var dep in subdepartments)
        {
          var children = GetEmployees(dep.ID, departments, empl);
          employees.AddRange(children);
        }
      }
      return employees;
    }
  }
}
