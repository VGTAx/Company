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

       

        public IActionResult Details(int? departmentId = 0, string? departmentName = null)
        {
            if (departmentName == null && departmentId == null)
            {
                return NotFound();
            }
            else if (departmentId == 0 && !String.IsNullOrEmpty(departmentName))
            {
                departmentId = _context.Departments.FirstOrDefault(d => d.DepartmentName == departmentName).ID;
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

            //var departments = _context.Departments.ToList();
            //var employee = _context.Employees;
           
            //ViewBag.Departments = departments;

            //return View("Delivery", employee);
        }

        public IActionResult Delivery()
        {
            return View();
        }
        //Описание отдела с сотрудниками отдела/подотделов
        //public IActionResult DetailsDepartment(int? departmentId)
        //{
        //    if (departmentId == null)
        //    {
        //        return NotFound();
        //    }

        //    var employeesDepartment = _context.Employees.Where(e => e.DepartmentID == departmentId).ToList();
        //    if (employeesDepartment.Count == 0)
        //    {
        //        employeesDepartment.AddRange(GetEmployees(departmentId, _context.Departments.ToList(), _context.Employees.ToList()));
        //    }
           
            
        //    var departments = _context.Departments
        //        .Where(d => employeesDepartment.Select(e => e.DepartmentID).Contains(d.ID));
        //    var departmentsDescription = _context.DepartmentDescriptions
        //        .FirstOrDefault(d => d.DepartmentDescriptionID == departmentId).Description;

        //    var imageLink = _context.Departments
        //        .FirstOrDefault(d => d.ID == departmentId).DepartmentImageLink;

        //    ViewBag.Departments = departments;
        //    ViewBag.MainDepartment = departmentId;
        //    ViewData["DepartmentDescription"] = departmentsDescription;
        //    ViewData["DepartmentImageLink"] = imageLink;

        //    return View("Details", employeesDepartment);
        //}



        

        

        private List<Employee> GetEmployees(int? id, List<Department> departments, List<Employee> empl)
        {           
            var subdepartments = departments.Where(d => d.ParentDepartmentID == id).ToList();            
            var employees = new List<Employee>();

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
