using Company.Data;
using Company.IServices;
using Company.Models;
using Company.Models.Admin;
using Company.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер, предоставляющий функциональность администратора.
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
    public IActionResult Index()
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
      var user = await _userManager!.FindByIdAsync(id!)!;
      if(user == null)
      {
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }
      var userPrincipal = _signInManager!.CreateUserPrincipalAsync(user).Result;

      var userRoles = userPrincipal.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      var roles = _roleManager!.Roles
        .Where(r => !exceptRoles.Contains(r.Name!))
        .Select(r => r.Name)
        .ToList();

      var model = new AccessSettingsPoco
      {
        User = user,
        UserRoles = userRoles,
        Roles = roles!
      };

      return PartialView(model);
    }
    /// <summary>
    /// Отображает страницу настройки доступа для пользователя с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Partial View страницы настройки доступа.</returns>
    [HttpPost]
    public async Task<IActionResult> AccessSettings([FromForm] UserInfoModel model)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = await _userManager!.FindByIdAsync(model.Id!);
      if(user == null)
      {
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      if(!model.SelectedRoles.Contains("User"))
      {
        ModelState.AddModelError(string.Empty, "Роль User не может быть удалена");
        return BadRequest(ModelState);
      }

      var userRoles = await GetUserRolesAsync(user);

      if(!model.SelectedRoles.SequenceEqual(userRoles)) //сравнение текущих ролей пользователя и выбранных ролей из формы
      {
        await ChangeRoleAsync(user, userRoles, model.SelectedRoles); //меняем роли

        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        _changeRole!.SendNotification(user.Id); //отправляем уведомление о смене ролей

        return PartialView("_StatusMessage", "Данные изменены!");
      }

      var userList = _context.Users.ToList();
      return PartialView("UserList", userList);
    }
    /// <summary>
    /// Отображает Partial View списка пользователей.
    /// </summary>
    /// <returns>Partial View списка пользователей.</returns>
    [HttpGet]
    public async Task<IActionResult> UserList()
    {
      var users = await _userManager!.Users.ToListAsync();
      return PartialView(users);
    }

    /// <summary>
    /// Меняет список ролей пользователя
    /// </summary>
    /// <param name="user">Пользователь у которого меняется роль</param>
    /// <param name="userRoles">Текущий список ролей пользователя</param>
    /// <param name="newUserRoles">Новый список ролей пользователя</param>
    /// <returns></returns>
    private async Task ChangeRoleAsync(ApplicationUserModel user, List<string> userRoles, List<string> newUserRoles)
    {
      var rolesToAdd = newUserRoles.Except(userRoles);
      var rolesToRemove = userRoles.Except(newUserRoles!);

      foreach(var role in rolesToAdd)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager!.AddClaimAsync(user, claimRole);
      }

      foreach(var role in rolesToRemove)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager!.RemoveClaimAsync(user, claimRole);
      }
    }

    /// <summary>
    /// Возвращает список ролей пользователя
    /// </summary>
    /// <param name="user">Текущий пользователь</param>
    /// <returns>Список ролей пользователя</returns>
    private async Task<List<string>> GetUserRolesAsync(ApplicationUserModel user)
    {
      var userClaims = await _userManager!.GetClaimsAsync(user);
      var userRoles = userClaims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      return userRoles;
    }
  }
}
