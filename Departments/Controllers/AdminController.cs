using Company.Data;
using Company.Models;
using Company.Models.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Company.Controllers
{
  [Authorize(Policy = "AdminOnlyPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class AdminController : Controller
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly RoleManager<IdentityRole>? _roleManager;
    private readonly CompanyContext _context;
    private readonly SignInManager<ApplicationUserModel> _signInManager;

    public AdminController(UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager, CompanyContext context, SignInManager<ApplicationUserModel> signInManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _context = context;
      _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    { 
      return View();
    }
    [HttpGet]
    public async Task<IActionResult> AccessSettings(string? id)
    {
      var user = await _userManager?.FindByIdAsync(id!)!;
      if (user == null)
      {
        return BadRequest();
      }
      var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
      var userRoles = userPrincipal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      var roles = _roleManager.Roles.Select(r => r.Name).ToList();

      ViewData["userRoles"] = userRoles;
      ViewData["roles"] = roles;


      return PartialView(user);
    }

    [HttpPost]
    public async Task<IActionResult> AccessSettings([FromBody] UserInfoModel model)
    {			
      var user = await _userManager?.FindByIdAsync(model.Id);

      if (user == null)
      {
        return BadRequest();
      }

      var currentUserRoles = User.Claims
				.Where(c => c.Type == ClaimTypes.Role)
				.Select(c => c.Value)
				.ToList();

      var rolesToAdd = model.SelectedRoles.Except(currentUserRoles);
      var rolesToRemove = currentUserRoles.Except(model.SelectedRoles);

      if(currentUserRoles.SequenceEqual(model.SelectedRoles))
      {
				return View();
			}     

			foreach (var role in rolesToAdd)
			{        
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.AddClaimAsync(user, claimRole);        
      }

      foreach (var role in rolesToRemove)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.RemoveClaimAsync(user, claimRole);
      }

      user.SecurityStamp = Guid.NewGuid().ToString();
      await _userManager.UpdateAsync(user);
      
      var new_user = await _userManager?.FindByIdAsync(user.Id);
      await _signInManager.RefreshSignInAsync(new_user);

      var userPrincipal = await _signInManager.CreateUserPrincipalAsync(new_user);
      // Установка аутентификационных куки
      //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
      

      ViewData["userRoles"] = userPrincipal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      ViewData["roles"] = _roleManager.Roles.Select(r => r.Name).ToList(); 


			return PartialView(new_user);
    }

    [HttpGet]
    public async Task<IActionResult> UserList()
    {
      var users = _userManager.Users.ToList();
      return PartialView(users);
    }
  }
}
