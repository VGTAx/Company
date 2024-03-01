using Company.BaseClass;
using Company.Interfaces;
using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер для управления информацией о сотрудниках.
  /// </summary>
  [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class EmployeeController : Controller
  {
    private readonly ICompanyContext _context;
    private readonly ILogger<EmployeeController> _logger;
    private readonly EmployeeServiceBase<EmployeeModel> _employeeService;
    private readonly DepartmentServiceBase<DepartmentModel> _departmentService;

    /// <summary>
    /// Создает экземпляр класса <see cref="EmployeeController"/>.
    /// </summary>
    /// <param name="context">Контекст компании для доступа к данным сотрудников.</param>
    public EmployeeController(
      ICompanyContext context,
      ILogger<EmployeeController> logger,
      EmployeeServiceBase<EmployeeModel> employeeService,
      DepartmentServiceBase<DepartmentModel> departmentService)
    {
      _context = context;
      _logger = logger;
      _employeeService = employeeService;
      _departmentService = departmentService;
    }

    /// <summary>
    /// Метод действия для создания нового сотрудника в указанном отделе.
    /// </summary>
    /// <param name="departmentId">Идентификатор отдела, в котором будет создан сотрудник (необязательный).</param>
    /// <returns>View для создания сотрудника с доступными отделами.</returns>
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Create()
    {
      var departments = _departmentService.GetDepartmentsListItem();
      _logger.LogInformation("Create employee method. Getting departments.");

      ViewBag.Departments = departments;
      return View();
    }

    /// <summary>
    /// Метод действия для создания нового сотрудника на основе входных данных формы.
    /// </summary>
    /// <param name="employee">Модель сотрудника, полученная из формы.</param>
    /// <returns>
    /// Если модель данных валидна, перенаправляет на страницу со списком сотрудников.
    /// В противном случае возвращает View "Create" с данными формы и списком доступных отделов.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] EmployeeModel employee)
    {
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _employeeService.GetModelErrors(ModelState);
        _logger.LogInformation("Create employee has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return BadRequest(ModelState);
      }

      await _context.Employees.AddAsync(employee);
      await _context.SaveChangesAsync();

      _logger.LogInformation("Employee {id} has created", employee.ID);
      return RedirectToAction(nameof(Details));
    }

    /// <summary>
    /// Метод  для редактирования данных сотрудника на основе указанного идентификатора.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника для редактирования.</param>
    /// <returns>
    /// Если сотрудник с указанным идентификатором найден, возвращает View "Edit" с данными сотрудника и списком доступных отделов.
    /// В противном случае возвращает NotFound().
    /// </returns>
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(int? id)
    {
      if(id == null)
      {
        _logger.LogInformation("Edit employee view has not gotten. Id is null");
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      if(employee == null)
      {
        _logger.LogWarning("Edit employee view has not gotten. Employee {id} not found", id);
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var departments = _departmentService.GetDepartmentsListItem();
      ViewBag.Departments = departments;

      return View(nameof(Edit), employee);
    }

    /// <summary>
    /// Метод для обновления данных сотрудника на основе указанного идентификатора.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника для обновления.</param>
    /// <param name="employee">Модель сотрудника с обновленными данными.</param>
    /// <returns>
    /// Если сотрудник с указанным идентификатором не найден, возвращает NotFoundResult.
    /// Если модель данных сотрудника валидна и обновление данных выполнено успешно, перенаправляет на метод действия "Details".
    /// В противном случае возвращает View "Edit" с моделью сотрудника и списком доступных отделов.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(int? id, [FromForm] EmployeeModel employee)
    {
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _employeeService.GetModelErrors(ModelState);
        _logger.LogInformation("Edit employee has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return BadRequest(ModelState);
      }

      if(id != employee.ID)
      {
        _logger.LogWarning("Edit employee has failed. Employee {id} not found", id);
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      try
      {
        _context.Update(employee);
        await _context.SaveChangesAsync();
      }
      catch(DbUpdateConcurrencyException)
      {
        if(!await _employeeService.IsEmployeeExist(employee.ID))
        {
          _logger.LogError("Employee {id} doesn't exist", employee.ID);
          return View("_StatusMessage", "Ошибка!Пользователь не найден.");
        }
        else
        {
          throw;
        }
      }

      _logger.LogInformation("Employee witg ID {id} has edited", employee.ID);
      return RedirectToAction(nameof(Details));
    }

    /// <summary>
    /// Удаляет сотрудникам с заданным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника, который необходимо удалить.</param>
    /// <returns>View со страницей подтверждения удаления сотрудника</returns>
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int? id)
    {
      if(id == null || _context.Employees == null)
      {
        _logger.LogWarning("Delete employee view has not gotten. Employee Id is null");
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      if(employee == null)
      {
        _logger.LogWarning("Delete employee view has not gotten. Employee with ID {Id} not found", id);
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var department = await _departmentService.GetDepartmentAsync(employee.DepartmentID);
      ViewBag.Department = department!.DepartmentName;

      return View(employee);
    }

    /// <summary>
    /// Метод подтверждения удаления сотрудника с заданным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника, который необходимо удалить.</param>
    /// <returns>При успешном удалении перенаправляет на действие Details</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if(_context.Employees == null)
      {
        _logger.LogError("Employee delete has failed. Entity set 'DBContext.Employee' is null.");
        return Problem("Entity set 'DepartmentContext.Employee' is null.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      if(employee != null)
      {
        _context.Employees.Remove(employee);
        _logger.LogInformation("Employee with ID {id} has deleted", id);
      }

      await _context.SaveChangesAsync();
      _logger.LogInformation("DBContext save changes");

      return RedirectToAction(nameof(Details));
    }

    /// <summary>
    /// Метод действия для просмотра списка сотрудников.
    /// </summary>
    /// <returns>
    /// Возвращает View Details со списком сотрудников.
    /// </returns>
    [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Details()
    {
      var employee = await _employeeService.GetEmployeesAsync();
      ViewBag.Departments = await _departmentService.GetDepartmentsAsync();

      return View(employee);
    }

    /// <summary>
    /// Возвращает View.
    /// </summary>
    /// <returns>View.</returns>
    public IActionResult Index()
    {
      return View();
    }
  }
}
