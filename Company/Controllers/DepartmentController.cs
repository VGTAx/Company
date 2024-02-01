using Company.Interfaces;
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
    private readonly ICompanyContext _context;
    /// <summary>
    /// Создает экземпляр класса <see cref="DepartmentController"/>.
    /// </summary>
    /// <param name="context">Контекст компании для доступа к данным отделов.</param>
    public DepartmentController(ICompanyContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Отображает список отделов и информацию о количестве сотрудников в каждом отделе.
    /// </summary>
    /// <returns>View с данными об отделах и количестве сотрудников.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
      var numberOfEmployyes = await _context.Departments.ToListAsync();

      return View(numberOfEmployyes);
    }

    /// <summary>
    /// Отображает информацию о выбранном отделе.
    /// </summary>
    /// <param name="departmentId">Идентификатор отдела.</param>
    /// <param name="departmentName">Название отдела.</param>
    /// <returns>View с информацией о выбранном отделе.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? departmentId = 0, string? departmentName = "")
    {
      if(String.IsNullOrEmpty(departmentName) && departmentId == null)
      {
        return View("_StatusMessage", "Ошибка! Отдел не найден.");
      }
      else if(departmentId == 0 && !String.IsNullOrEmpty(departmentName))
      {
        var tempDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
        departmentId = tempDepartment!.ID;
      }
      var department = _context.Departments.FirstOrDefault(d => d.ID == departmentId);

      return departmentId switch
      {
        1 => View("CustomerService", department),
        2 => View("Production", department),
        3 => View("Bookkeeping", department),
        4 => View("Sales", department),
        5 => View("WholeSales", department),
        6 => View("RetailSales", department),
        7 => View("Logistic", department),
        8 => View("Stock", department),
        9 => View("Delivery", department),
        10 => View("Engineering", department),
        11 => View("QualityControl", department),
        12 => View("Purchasing", department),
        _ => NotFound(),
      };
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
      var subdepartments = departments
                               .Where(d => d.ParentDepartmentID == id)
                               .ToList();
      var employees = new List<EmployeeModel>();

      if(subdepartments.Count != 0)
      {
        employees = empl.Where(e => subdepartments.Select(s => s.ID)
                            .Contains(e.ID))
                        .ToList();
      }
      else
      {
        employees = empl.Where(e => e.DepartmentID == id).ToList();
      }

      if(subdepartments.Any())
      {
        foreach(var dep in subdepartments)
        {
          var children = GetEmployees(dep.ID, departments, empl);
          employees.AddRange(children);
        }
      }
      return employees;
    }
  }
}
