using Microsoft.AspNetCore.Mvc;
using Company.Data;
using Microsoft.EntityFrameworkCore;
using Company.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;

namespace Company.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentContext _context;

        public DepartmentController(DepartmentContext context)
        {
            _context = context;
        }
        

        public async Task<IActionResult> Index()
        {
            var numberOfEmployyes = from d in _context.NumberOfEmployees
                                    join e in _context.Departments on d.DepartmentID equals e.ID
                                    select new DepartmentNumberPoco(e.ID, e.DepartmentName,
                                                                e.ParentDepartmentID, d.EmployeeCount);
                                   

            return View(await numberOfEmployyes.ToListAsync());
        }
       
        
        public IActionResult Create()
        {
            var departments = _context.Departments
                .Where(d => !_context.Departments.Any(sub => sub.ParentDepartmentID == d.ID))
                .Select(item=> new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.DepartmentName
                });           

            ViewData["Departments"] = departments;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID, Name, Surname, Age, Number, DepartmentID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id); 

            if(employee == null)
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

            return View("Edit",employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ID, Name, Surname, Age, Number, DepartmentID")] Employee employee)
        {
            if(id != employee.ID)
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

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee =  await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);

            if(employee == null)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_context.Employees == null)
            {
                return Problem("Entity set 'DepartmentContext.Employee' is null.");
            }

            var employee = _context.Employees.FirstOrDefault(e => e.ID == id);
           
            if(employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details()
        {
            var departments = _context.Departments.ToList();
            var employee = _context.Employees;

           
            ViewBag.Departments = departments;

            return View(employee);
        }

        public IActionResult DetailsDepartment(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var employeesDepartment = _context.Employees.Where(e => e.DepartmentID == id).ToList();
            employeesDepartment.AddRange(GetEmployees(id, _context.Departments.ToList(), _context.Employees.ToList()));

            var departments = _context.Departments
                .Where(d => employeesDepartment.Select(e => e.DepartmentID).Contains(d.ID));

            ViewBag.Departments = departments;

            return View("Details", employeesDepartment);
        }

        

        private bool EmployeeExists(int? id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }

        private List<Employee> GetEmployees(int? id, List<Department> departments, List<Employee> empl)
        {
            
            var subdepartments = departments.Where(d=>d.ParentDepartmentID == id).ToList();
            

            var employees = empl.Where(e => subdepartments.Select(s => s.ID).Contains(e.ID))
                                            .ToList();
            foreach(var employee in employees)
            {
                Console.WriteLine($"{employee.Name}\n");
            }


            if (subdepartments.Any())
            {
                foreach (var dep in subdepartments)
                {
                    Console.WriteLine(dep.DepartmentName);

                    var children = GetEmployees(dep.ID, departments, empl);
                    Console.WriteLine(children.Count);
                    employees.AddRange(children);
                }
            }
            Console.WriteLine($"count empl: {employees.Count}\nDepar count: {subdepartments.Count}");
            return employees;
        }
    }
}
