using Company.Data;
using Company.IServices;
using Company.Models;
using Company.Models.Admin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер, предоставляющий функциональность администратора.
  /// Для доступа к контроллеру и его действиям требуется аутентификация через куки и соответствие политике "AdminPolicy".
  /// </summary>
  [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class AdminController : Controller
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly RoleManager<IdentityRole>? _roleManager;
    private readonly SignInManager<ApplicationUserModel>? _signInManager;
    private readonly INotificationService? _changeRole;
    private readonly CompanyContext _context;
    private readonly List<string> exceptRoles = new List<string> { "Admin" }!;

    /// <summary>
    /// Создает экземпляр класса <see cref="AdminController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей.</param>
    /// <param name="roleManager">Менеджер ролей.</param>
    /// <param name="signInManager">Менеджер входа в систему.</param>
    /// <param name="changeRole">Сервис уведомлений о смене роли.</param>
    /// <param name="context">Контекст базы данных компании.</param>
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
    /// <summary>
    /// Отображает главную страницу администратора.
    /// </summary>
    /// <returns>View главной страницы администратора.</returns>
    public async Task<IActionResult> Index()
    {
      return View();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> AccessSettings(string? id)
    {
      var user = await _userManager?.FindByIdAsync(id!)!;
      if(user == null)
      {
        return BadRequest("User not found");
      }
      var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

      ViewBag.UserRoles = userPrincipal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      ViewBag.Roles = _roleManager.Roles
        .Where(r => !exceptRoles.Contains(r.Name))
        .Select(r => r.Name)
        .ToList();


      return PartialView(user);
    }
    /// <summary>
    /// Отображает страницу настройки доступа для пользователя с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Partial View страницы настройки доступа.</returns>
    [HttpPost]
    public async Task<IActionResult> AccessSettings([FromBody] UserInfoModel model)
    {
      var user = await _userManager?.FindByIdAsync(model.Id);

      if(user == null)
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

      if(!model.SelectedRoles.Contains("User"))
      {
        ViewBag.StatusMessage = "Ошибка! Роль User не может быть удалена!"!;
        return PartialView("_StatusMessage", ViewBag.StatusMessage);
      }

      foreach(var role in rolesToAdd)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.AddClaimAsync(user, claimRole);
        ViewBag.StatusMessage = "Данные изменены!"!;
      }

      foreach(var role in rolesToRemove)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager.RemoveClaimAsync(user, claimRole);
        ViewBag.StatusMessage = "Данные изменены!"!;
      }

      if(ViewBag.StatusMessage?.ToString() == "Данные изменены!"!)
      {
        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        _changeRole.SendNotification(user.Id);
        return PartialView("_StatusMessage", ViewBag.StatusMessage);
      }
      var userList = _context.Users.ToList();
      return PartialView("UserList", userList);

    }
    /// <summary>
    /// Отображает Partial View списка пользователей.
    /// </summary>
    /// <returns>Partial View списка пользователей.</returns>
    [HttpGet]
    public IActionResult UserList()
    {
      var users = _userManager.Users.ToList();
      return PartialView(users);
    }
  }
}
