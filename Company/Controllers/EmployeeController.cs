using Company.BaseClass;
using Company.Interfaces;
using Company.Models.Departments;
using Company.Models.Employee;
using Company.Services;
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
    private readonly EmployeeBase<EmployeeModel> _employeeService;
    private readonly DepartmentBase<DepartmentModel> _departmentService;
    private readonly StringParser _stringParser;

    /// <summary>
    /// Создает экземпляр класса <see cref="EmployeeController"/>.
    /// </summary>
    /// <param name="context">Контекст компании для доступа к данным сотрудников.</param>
    public EmployeeController(
      ICompanyContext context,
      ILogger<EmployeeController> logger,
      EmployeeBase<EmployeeModel> employeeService,
      DepartmentBase<DepartmentModel> departmentService,
      StringParser stringParser)
    {
      _context = context;
      _logger = logger;
      _employeeService = employeeService;
      _departmentService = departmentService;
      _stringParser = stringParser;
    }

    private void Logger(LogLevel logLevel, string methodName, string message, string? employeeId = default)
    {
      var ip = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
      _logger.Log(logLevel, ": {IP} {method} - {employeeId} {message}", ip, methodName, employeeId, message);
    }

    /// <summary>
    /// Метод действия для создания нового сотрудника в указанном отделе.
    /// </summary>
    /// <param name="departmentId">Идентификатор отдела, в котором будет создан сотрудник (необязательный).</param>
    /// <returns>View для создания сотрудника с доступными отделами.</returns>
    [Authorize(Policy = "ManagePolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Create()
    {
      var methodName = nameof(Create);
      var departments = _departmentService.GetDepartmentsListItem();

      ViewBag.Departments = departments;
      return View(methodName);
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
      var methodName = nameof(Create);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _employeeService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {errors}");
        return BadRequest(ModelState);
      }

      await _context.Employees.AddAsync(employee);
      await _context.SaveChangesAsync();

      Logger(LogLevel.Information, methodName, "Employee created", employee.ID.ToString());
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
    public async Task<IActionResult> EditEmployee(int? id)
    {
      var methodName = nameof(EditEmployee);

      if(id == null)
      {
        Logger(LogLevel.Error, methodName, "Employee id is null");
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      if(employee == null)
      {
        Logger(LogLevel.Warning, methodName, "Employee not found", id.ToString());
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var departments = _departmentService.GetDepartmentsListItem();
      ViewBag.Departments = departments;

      return View(nameof(EditEmployee), employee);
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
    public async Task<IActionResult> EditEmployeePost(int? id, [FromForm] EmployeeModel employee)
    {
      var methodName = nameof(EditEmployeePost);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _employeeService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {errors}");
        return BadRequest(ModelState);
      }

      if(id != employee.ID)
      {
        Logger(LogLevel.Warning, methodName, $"Employee not found");
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      try
      {
        _context.Update(employee);
        await _context.SaveChangesAsync();
        Logger(LogLevel.Information, methodName, "Employee info changed", employee.ID.ToString());
      }
      catch(DbUpdateConcurrencyException)
      {
        if(!await _employeeService.IsEmployeeExist(employee.ID))
        {
          Logger(LogLevel.Error, methodName, "Employee not exist");
          return View("_StatusMessage", "Ошибка!Пользователь не найден.");
        }
        else
        {
          Logger(LogLevel.Critical, methodName, "Db Update Concurrency Exception");
          throw;
        }
      }

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
      var methodName = nameof(Delete);

      if(id == null || _context.Employees == null)
      {
        Logger(LogLevel.Error, methodName, "Employee Id is null");
        return View("_StatusMessage", "Ошибка!Пользователь не найден.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      if(employee == null)
      {
        Logger(LogLevel.Warning, methodName, "Employee not found", id.ToString());
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
      var methodName = nameof(DeleteConfirmed);

      if(_context.Employees == null)
      {
        Logger(LogLevel.Critical, methodName, "Entity set 'DBContext.EmployeeModel' is null.");
        return Problem("Entity set 'DepartmentContext.EmployeeModel' is null.");
      }

      var employee = await _employeeService.GetEmployeeAsync(id);

      _context.Employees.Remove(employee);
      if(await _context.SaveChangesAsync() != 0)
      {
        Logger(LogLevel.Information, methodName, "Employee deleted", id.ToString());
        return RedirectToAction(nameof(Details));
      }

      _context.Employees.Add(employee);
      Logger(LogLevel.Information, methodName, "Employee deleted error", id.ToString());
      return RedirectToAction(nameof(DeleteConfirmed), id);
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
      var methodName = nameof(Details);
      var employee = await _employeeService.GetEmployeesAsync();
      ViewBag.Departments = await _departmentService.GetDepartmentsAsync();

      Logger(LogLevel.Information, methodName, "Get employees list");

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
