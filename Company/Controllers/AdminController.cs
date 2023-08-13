using Company_.Data;
using Company_.IServices;
using Company_.Models;
using Company_.Models.Admin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Company_.Controllers
{
  [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class AdminController : Controller
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly RoleManager<IdentityRole>? _roleManager;
    private readonly SignInManager<ApplicationUserModel> _signInManager;
    private readonly INotificationService _changeRole;
    private readonly CompanyContext _context;
    private readonly List<string> exceptRoles = new List<string> { "Admin" }!;

    public AdminController(UserManager<ApplicationUserModel> userManager,
      RoleManager<IdentityRole> roleManager,
      SignInManager<ApplicationUserModel> signInManager,
      INotificationService changeRole,
      CompanyContext context)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _signInManager = signInManager;
      _changeRole = changeRole;
      _context = context;
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
        return BadRequest("User not found");
      }
      var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

      ViewData["userRoles"] = userPrincipal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      ViewData["roles"] = _roleManager.Roles
        .Where(r => !exceptRoles.Contains(r.Name))
        .Select(r => r.Name)
        .ToList();


      return PartialView(user);
    }

    [HttpPost]
    public async Task<IActionResult> AccessSettings([FromBody] UserInfoModel model)
    {
      var user = await _userManager?.FindByIdAsync(model.Id);

      if (user == null)
      {
        return BadRequest("User not found");
      }

      var userClaims = await _userManager.GetClaimsAsync(user);
      var userRoles = userClaims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      var rolesToAdd = model.SelectedRoles.Except(userRoles);
      var rolesToRemove = userRoles.Except(model.SelectedRoles);

      if (!model.SelectedRoles.Contains("User"))
      {
        ViewData["StatusMessage"] = "Ошибка! Роль User не может быть удалена!"!;
        return PartialView("_StatusMessage", ViewData["StatusMessage"]);
      }

      foreach (var role in rolesToAdd)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.AddClaimAsync(user, claimRole);
        ViewData["StatusMessage"] = "Данные изменены!"!;
      }

      foreach (var role in rolesToRemove)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.RemoveClaimAsync(user, claimRole);
        ViewData["StatusMessage"] = "Данные изменены!"!;
      }

      if (ViewData["StatusMessage"]?.ToString() == "Данные изменены!"!)
      {
        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        _changeRole.SendNotification(user.Id);
        return PartialView("_StatusMessage", ViewData["StatusMessage"]);
      }
      var userList = _context.Users.ToList();
      return PartialView("UserList", userList);

    }

    [HttpGet]
    public async Task<IActionResult> UserList()
    {
      var users = _userManager.Users.ToList();
      return PartialView(users);
    }
  }
}
