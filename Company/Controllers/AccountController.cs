using Company.Models;
using Company.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер для управления учетными записями пользователей.
  /// </summary>
  public class AccountController : Controller
  {
    private readonly SignInManager<ApplicationUserModel> _signInManager;
    private readonly UserManager<ApplicationUserModel> _userManager;
    private readonly IUserStore<ApplicationUserModel> _userStore;
    private readonly IUserEmailStore<ApplicationUserModel> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;
    /// <summary>
    /// Конструктор контроллера управления учетными записями пользователей.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей для работы с учетными записями.</param>
    /// <param name="userStore">Хранилище пользователей.</param>
    /// <param name="signInManager">Менеджер аутентификации и входа в систему.</param>
    /// <param name="logger">Логгер для записи событий и ошибок.</param>
    /// <param name="emailSender">Сервис отправки электронных писем.</param>
    /// <param name="roleManager">Менеджер ролей для работы с ролями пользователей.</param>
    public AccountController(
        UserManager<ApplicationUserModel> userManager,
        IUserStore<ApplicationUserModel> userStore,
        SignInManager<ApplicationUserModel> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManger)
    {
      _userManager = userManager;
      _userStore = userStore;
      _emailStore = GetEmailStore()!;
      _signInManager = signInManager;
      _logger = logger;
      _emailSender = emailSender;
      _roleManager = roleManger;
    }
    /// <summary>
    /// Отображает страницу регистрации для пользователей.
    /// </summary>
    /// <returns>ViewResult c gредставлением страницы регистрации.</returns>
    [HttpGet]
    public IActionResult Registration()
    {
      return View();
    }
    /// <summary>
    /// Обрабатывает POST-запрос для регистрации пользователя.
    /// </summary>
    /// <param name="model">Модель регистрации, содержащая данные пользователя.</param>
    /// <returns>Редирект на страницу подтверждения при успешной регистрации или страницу регистрации с ошибками.</returns>
    [HttpPost]
    public async Task<IActionResult> Registration([Bind("Email, Name, Password, ConfirmPassword")] RegisterModel model)
    {
      if (ModelState.IsValid)
      {
        var user = new ApplicationUserModel
        {
          UserName = model.Email,
          Email = model.Email,
          Name = model.Name,
        };

        await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (result.Succeeded)
        {
          _logger.LogInformation("User created a new account with password.");

          var claim = new Claim("Name", model.Name, ClaimValueTypes.String);
          await _userManager.AddClaimAsync(user, claim);
          var role = await _roleManager.FindByNameAsync("User");
          var roleClaim = new Claim(ClaimTypes.Role, role.Name);

          await _userManager.AddClaimAsync(user, roleClaim);
          var userId = await _userManager.GetUserIdAsync(user);
          var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

          var callBackUrl = Url.Action(
              action: "RegistrationConfirmation",
              controller: "Account",
              values: new { userId = userId, code = code, statusConfirmation = "true" },
              protocol: Request.Scheme);

          await _userManager.AddToRoleAsync(user, "user");


          await _emailSender.SendEmailAsync(model.Email, "Подтверждение регистрации",
              $"Спасибо за регистрацию. Пожалуйста, перейдите по ссылке , чтобы подтвердить ваш адрес электронной почты: <a href='{HtmlEncoder.Default.Encode(callBackUrl!)}' id='confrimationLink'>нажмите сюда</a>." +
              $"\n\nЕсли вы получили это письмо случайно - удалите это письмо.");

          if (_userManager.Options.SignIn.RequireConfirmedAccount)
          {
            return RedirectToAction("RegistrationConfirmation", "Account");
          }
          else
          {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect("/");
          }
        }

        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }
      return View();
    }
    /// <summary>
    /// Обрабатывает подтверждение регистрации пользователя по электронной почте.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="code">Код подтверждения из электронной почты.</param>
    /// <returns>
    /// Возвращает страницу с сообщением об успешном завершении регистрации или сообщение об ошибке.
    /// </returns>
    public async Task<IActionResult> RegistrationConfirmation(string userId, string code)
    {
      if (userId == null || code == null)
      {
        ViewBag.StatusMessage = $"Пожалуйста, проверьте электронную почту, чтобы подтвердить свою учетную запись.";
        return View();
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userId}'.");
      }

      code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

      var result = await _userManager.ConfirmEmailAsync(user, code);
      ViewBag.StatusMessage = result.Succeeded ? "Регистрация завершена." : "Ошибка при подтверждении эл.почты";

      return View();
    }
    /// <summary>
    /// Отображает страницу входа для пользователей.
    /// </summary>
    /// <returns>ViewResult с представлением страницы входа.</returns>
    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }
    /// <summary>
    /// Обрабатывает POST-запрос для аутентификации пользователя.
    /// </summary>
    /// <param name="model">Модель аутентификации, содержащая данные пользователя.</param>
    /// <returns>
    /// Если аутентификация успешна, выполняется вход пользователя и перенаправление на главную страницу.
    /// В противном случае, возвращает страницу входа с сообщением об ошибке.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Login([Bind("Email, Password, RememberMe")] LoginModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
          var user = await _userManager.FindByEmailAsync(model.Email);
          // Создание ClaimsPrincipal на основе пользователя
          var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

          // Установка аутентификационных куки
          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

          _logger.LogInformation("User logged in.");
          return RedirectToAction("Index", "Department");
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return View();
        }
      }

      return View();
    }
    /// <summary>
    /// Выполняет выход пользователя из системы.
    /// </summary>
    /// <returns>
    /// Разлогинивает пользователя и перенаправляет на главную страницу.
    /// </returns>
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      _logger.LogInformation("User logged out.");
      return RedirectToAction("Index", "Department");
    }
    /// <summary>
    /// Отображает страницу для восстановления пароля.
    /// </summary>
    /// <returns>ViewResult с представлением страницы восстановления пароля.</returns>
    [HttpGet]
    public IActionResult ForgotPassword()
    {
      return View();
    }
    /// <summary>
    /// Обрабатывает POST-запрос для восстановления пароля пользователя.
    /// </summary>
    /// <param name="model">Модель восстановления пароля, содержащая адрес электронной почты.</param>
    /// <returns>
    /// Если запрос успешно обработан, отправляет письмо для восстановления пароля и перенаправляет на страницу подтверждения.
    /// В противном случае, возвращает страницу восстановления пароля с сообщением об ошибке.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> ForgotPassword([Bind("Email")] ForgotPasswordModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
          ModelState.AddModelError(string.Empty, "Пользователя с такой электронной почтой не существует.");
          return View();
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callBackUrl = Url.Action(
            action: "ResetPassword",
            controller: "Account",
            values: new { code = code, Email = model.Email },
            protocol: Request.Scheme);
        await _emailSender.SendEmailAsync(model.Email!, "Восстановление пароля",
            $"Для восстановления пароля перейдите по ссылке : <a href = '{HtmlEncoder.Default.Encode(callBackUrl!)}'>нажмите сюда</a >.");

        return RedirectToAction("ForgotPasswordConfirmation", "Account");
      }

      return View();
    }
    /// <summary>
    /// Отображает страницу подтверждения восстановления пароля.
    /// </summary>
    /// <returns>ViewResult с представлением страницы подтверждения восстановления пароля.</returns>
    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
      return View();
    }
    /// <summary>
    /// Отображает страницу сброса пароля.
    /// </summary>
    /// <param name="code">Код сброса пароля.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <returns>
    /// Возвращает страницу сброса пароля или ошибку BadRequest.
    /// </returns>
    [HttpGet]
    public IActionResult ResetPassword(string code = null, string email = null)
    {
      if (code == null || email == null)
      {
        return BadRequest("A code must be supplied for password reset.");
      }

      return View();
    }
    /// <summary>
    /// Обрабатывает POST-запрос для сброса пароля пользователя.
    /// </summary>
    /// <param name="model">Модель с данными для сброса пароля.</param>
    /// <returns>
    /// Если данные действительны и пароль успешно сброшен, перенаправляет на страницу подтверждения сброса пароля.
    /// В противном случае, возвращает страницу с сообщениями об ошибках.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> ResetPassword([Bind("Email, Password, ConfirmPassword, Code")] ResetPasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
      {
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }
      model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
      var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View();
    }
    /// <summary>
    /// Отображает страницу подтверждения сброса пароля.
    /// </summary>
    /// <returns>ViewResult с представлением страницы подтверждения сброса пароля.</returns>
    public IActionResult ResetPasswordConfirmation()
    {
      return View();
    }
    /// <summary>
    /// Получает интерфейс хранилища электронной почты для пользователей.
    /// </summary>
    /// <returns>
    /// Интерфейс хранилища электронной почты для пользователей.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Выбрасывается, если хранилище пользователей не поддерживает адреса электронной почты.
    /// </exception>
    private IUserEmailStore<ApplicationUserModel>? GetEmailStore()
    {
      if (!_userManager.SupportsUserEmail)
      {
        throw new NotSupportedException("The default UI requires a user store with email support.");
      }
      return (IUserEmailStore<ApplicationUserModel>)_userStore;
    }
  }
}
