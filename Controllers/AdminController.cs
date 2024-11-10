using Company.Data;
using Company.Interfaces;
using Company.IServices;
using Company.Models;
using Company.Models.Admin;
using Company.Models.ViewModels;
using Company.Services;
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
    private readonly INotification? _changeRole;
    private readonly IUserRoleClaims<AppUser> _userRoleClaims;
    private readonly CompanyContext _context;
    private readonly UserManager<AppUser>? _userManager;
    private readonly RoleManager<IdentityRole>? _roleManager;
    private readonly StringParser _stringParser;

    private readonly List<string> exceptRoles = new List<string> { "Admin" }!;

    private void Logger(LogLevel logLevel, string methodName, string message, string userId)
    {
      var ip = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
      _logger.Log(logLevel, ": {IP} {method} - {user} {message}", ip, methodName, userId, message);
    }

    private void Logger(LogLevel logLevel, string methodName, string message)
    {
      var ip = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
      _logger.Log(logLevel, ": {IP} {method} - {message}", ip, methodName, message);
    }

    /// <summary>
    /// Создает экземпляр класса <see cref="AdminController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей.</param>
    /// <param name="roleManager">Менеджер ролей.</param>
    /// <param name="signInManager">Менеджер входа в систему.</param>
    /// <param name="changeRole">Сервис уведомлений о смене роли.</param>
    /// <param name="context">Контекст базы данных компании.</param>
    public AdminController(
      ILogger<AdminController> logger,
      INotification changeRole,
      IUserRoleClaims<AppUser> userRoleClaims,
      CompanyContext context,
      UserManager<AppUser> userManager,
      RoleManager<IdentityRole> roleManager,
      StringParser errorParser
      )
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _changeRole = changeRole;
      _userRoleClaims = userRoleClaims;
      _context = context;
      _logger = logger;
      _stringParser = errorParser;
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
      var methodName = nameof(AccessSettings);

      if(String.IsNullOrEmpty(id))
      {
        Logger(LogLevel.Warning, methodName, "UserId is null");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      var user = await _userManager!.FindByIdAsync(id);

      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
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

      Logger(LogLevel.Information, methodName, $"Got access settings for user {user.Id}");
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
      var methodName = nameof(AccessSettings);

      var user = await _userManager!.FindByIdAsync(model.Id!);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found", model.Id!);
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден!");
      }

      if(!ModelState.IsValid)
      {
        var modelStateErrors = ModelState.Values
          .SelectMany(c => c.Errors)
          .Select(c => c.ErrorMessage);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Changed settings failed. Model isn't valid. Errors: {errors}");
        return BadRequest(ModelState);
      }

      if(!model.SelectedRoles!.Contains("User"))
      {
        ModelState.AddModelError(string.Empty, "Роль User не может быть удалена");
        Logger(LogLevel.Error, methodName, $"Claims \"User\" can't be deleted", user.Id);

        return BadRequest(ModelState);
      }

      var userRoles = await _userRoleClaims.GetUserRoleClaimsAsync(user);

      if(!model.SelectedRoles.SequenceEqual(userRoles)) //сравнение текущих ролей пользователя и выбранных ролей из формы
      {
        var roles = _stringParser.CollectionToString(model.SelectedRoles);
        Logger(LogLevel.Information, methodName, $"Change user role claims to: {roles}", user.Id);

        await _userRoleClaims.ChangeUserRoleClaimsAsync(user, userRoles, model.SelectedRoles); //меняем роли

        user.SecurityStamp = Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        _changeRole!.SendNotification(user.Id); //отправляем уведомление о смене ролей
        Logger(LogLevel.Information, methodName, $"Update user role claims", user.Id);

        return PartialView("_StatusMessage", "Данные изменены!");
      }

      var userList = _context.Users.ToList();
      return PartialView(nameof(UserList), userList);
    }
    /// <summary>
    /// Отображает Partial View списка пользователей.
    /// </summary>
    /// <returns>Partial View списка пользователей.</returns>
    [HttpGet]
    public async Task<IActionResult> UserList()
    {
      var methodName = nameof(UserList);
      var users = await _context!.Users.ToListAsync();

      Logger(LogLevel.Information, methodName, "Get user list.");
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