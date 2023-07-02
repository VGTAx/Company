using Company.Data;
using Company.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Company.Models.Department;
using Company.Models.Employee;
using Company.Models;
using Microsoft.AspNetCore.Identity;

namespace Company.Controllers
{

    public class DepartmentController : Controller
    {
        private readonly CompanyContext _context;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;


        public DepartmentController(CompanyContext context, UserManager<ApplicationUserModel> userManager, SignInManager<ApplicationUserModel> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public async Task<IActionResult> _LoginPartial()
        //{
            
        //    return PartialView(User);
        //}

        public async Task<IActionResult> Index()
        {
            var numberOfEmployyes = from d in _context.NumberOfEmployees
                                    join e in _context.Departments on d.DepartmentID equals e.ID
                                    select new DepartmentNumberPoco(e.ID, e.DepartmentName,
                                                                e.ParentDepartmentID, d.EmployeeCount);

            //var model = new IndexViewModel
            //{
            //    departmentNumberPoco = await numberOfEmployyes.ToListAsync(),
            //    applicationUser = await _userManager.GetUserAsync(User),
            //};

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

            //var departments = _context.Departments.ToList();
            //var employee = _context.Employees;

            //ViewBag.Departments = departments;

            //return View("Delivery", employee);
        }

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
