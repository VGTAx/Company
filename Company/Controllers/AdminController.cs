using Company.Data;
using Company.Interfaces;
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

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер, предоставляющий функциональность администратора.
  /// </summary>
  [Authorize(Policy = "AdminPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class AdminController : Controller
  {
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly RoleManager<IdentityRole>? _roleManager;
    private readonly INotificationService? _changeRole;
    private readonly IUserRoleClaims<ApplicationUserModel> _userRoleClaims;
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
    public AdminController(
      UserManager<ApplicationUserModel> userManager,
      RoleManager<IdentityRole> roleManager,
      INotificationService changeRole,
      IUserRoleClaims<ApplicationUserModel> userRoleClaims,
      CompanyContext context,
      ILogger<AdminController> logger)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _changeRole = changeRole;
      _userRoleClaims = userRoleClaims;
      _context = context;
      _logger = logger;
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
      if(String.IsNullOrEmpty(id))
      {
        _logger.LogInformation("Access settings aren't available. Id is null");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      var user = await _userManager!.FindByIdAsync(id);

      if(user == null)
      {
        _logger.LogWarning("Access settings aren't available. User {id} not found", id);
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      var userRoles = await _userRoleClaims.GetUserRoleClaimsAsync(user);
      var roles = await GetRolesAsync();

      var model = new AccessSettingsPoco
      {
        User = user,
        UserRoles = userRoles,
        Roles = roles!
      };

      _logger.LogInformation("Access settings for User {id} are gotten", id);
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
        var modelStateErrors = ModelState.Values.SelectMany(c => c.Errors)
                                .Select(c => c.ErrorMessage);

        _logger.LogInformation("Changing access settings is not available. " +
          "Model isn't valid. Errors: {errors}", modelStateErrors);
        return BadRequest(ModelState);
      }

      var user = await _userManager!.FindByIdAsync(model.Id!);
      if(user == null)
      {
        _logger.LogWarning("Changing access settings is not available. User {id} not found", user.Id);
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      if(!model.SelectedRoles.Contains("User"))
      {
        ModelState.AddModelError(string.Empty, "Роль User не может быть удалена");
        _logger.LogWarning("Changing access settings for user {id} is not available. " +
          "Claims \"User\" can't be deleted", user.Id);
        return BadRequest(ModelState);
      }

      var userRoles = await _userRoleClaims.GetUserRoleClaimsAsync(user);

      if(!model.SelectedRoles.SequenceEqual(userRoles)) //сравнение текущих ролей пользователя и выбранных ролей из формы
      {
        _logger.LogInformation("Changing user role claims for user {id}", user.Id);
        await _userRoleClaims.ChangeUserRoleClaimsAsync(user, userRoles, model.SelectedRoles); //меняем роли

        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("Send notification about changing user role claims.");

        _changeRole!.SendNotification(user.Id); //отправляем уведомление о смене ролей

        _logger.LogInformation("User role claims has changed for user {id}", user.Id);
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
      var users = await _context!.Users.ToListAsync();

      _logger.LogInformation("User List method. Getting user list.");
      return PartialView(users);
    }

    /// <summary>
    /// Возвращает список ролей
    /// </summary>
    /// <returns>Список ролей</returns>
    private async Task<List<string?>> GetRolesAsync()
    {
      return await _roleManager!.Roles
        .Where(r => !exceptRoles.Contains(r.Name!))
        .Select(r => r.Name)
        .ToListAsync();
    }
  }
}