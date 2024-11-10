using Company.BaseClass;
using Company.Interfaces;
using Company.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace Company.Services.Conrollers
{
  public class Employee : EmployeeBase<Models.Employee.EmployeeModel>
  {
    private readonly ICompanyContext _context;

    public Employee(ICompanyContext context)
    {
      _context = context;
    }

    public override async Task<Models.Employee.EmployeeModel> GetEmployeeAsync(int? id)
    {
      return id.HasValue ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == id) : null;
    }

    public override async Task<List<Models.Employee.EmployeeModel>> GetEmployeesAsync()
    {
      return await _context.Employees.ToListAsync();
    }

    /// <summary>
    /// Проверяет существование сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>True, если сотрудник с указанным идентификатором существует, иначе false.</returns>
    public override async Task<bool> IsEmployeeExist(int? id)
    {
      return id.HasValue && await _context.Employees.AnyAsync(e => e.Id == id);
    }
  }
}
