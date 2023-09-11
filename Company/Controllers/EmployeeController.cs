using Company.Data;
using Company.Models.Department;
using Company.Models.Employee;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер для управления информацией о сотрудниках.
  /// </summary>
  [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class EmployeeController : Controller
  {
    private readonly CompanyContext _context;
    /// <summary>
    /// Инициализирует новый экземпляр контроллера для управления сотрудниками с использованием указанного контекста компании.
    /// </summary>
    /// <param name="context">Контекст компании для доступа к данным сотрудников.</param>
    public EmployeeController(CompanyContext context)
    {
      _context = context;
    }
    /// <summary>
    /// Метод действия для создания нового сотрудника в указанном отделе.
    /// </summary>
    /// <param name="departmentId">Идентификатор отдела, в котором будет создан сотрудник (необязательный).</param>
    /// <returns>View для создания сотрудника с доступными отделами.</returns>
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
    /// <summary>
    /// Метод действия для создания нового сотрудника на основе входных данных формы.
    /// </summary>
    /// <param name="employee">Модель сотрудника, полученная из формы.</param>
    /// <returns>
    /// Если модель данных валидна, перенаправляет на страницу со списком сотрудников.
    /// В противном случае возвращает View "Create" с данными формы и списком доступных отделов.
    /// </returns>
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
    /// <summary>
    /// Удаляет сотрудникам с заданным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника, который необходимо удалить.</param>
    /// <returns>View со страницей подтверждения удаления сотрудника</returns>
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
    /// <summary>
    /// Метод действия для просмотра списка сотрудников.
    /// </summary>
    /// <returns>
    /// Возвращает View Details со списком сотрудников.
    /// </returns>
    [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]   
    public IActionResult Details()
    {
      var departments = _context.Departments.ToList();
      var employee = _context.Employees;

      ViewBag.Departments = departments;

      return View(employee);
    }
    /// <summary>
    /// Метод для получения списка отделов.
    /// </summary>
    /// <param name="id">Идентификатор отдела.</param>
    /// <param name="departments">Список отделов.</param>
    /// <returns>Список отделов.</returns>
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
    /// <summary>
    /// Проверяет существование сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>True, если сотрудник с указанным идентификатором существует, иначе false.</returns>
    private bool EmployeeExists(int? id)
    {
      return _context.Employees.Any(e => e.ID == id);
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
