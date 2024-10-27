using Company.BaseClass;
using Company.Interfaces;
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
    private readonly AccountBase _accountService;
    private readonly ICompanyContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IUserStore<AppUser> _userStore;
    private readonly IUserEmailStore<AppUser> _emailStore;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Создает экземпляр класса <see cref="AccountController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей для работы с учетными записями.</param>
    /// <param name="userStore">Хранилище пользователей.</param>
    /// <param name="signInManager">Менеджер аутентификации и входа в систему.</param>
    /// <param name="emailSender">Сервис отправки электронных писем.</param>
    /// <param name="roleManager">Менеджер ролей для работы с ролями пользователей.</param>    
    public AccountController(
        AccountBase accountService,
        ICompanyContext context,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        IUserStore<AppUser> userStore,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
        )
    {
      _userManager = userManager;
      _userStore = userStore;
      _emailStore = (IUserEmailStore<AppUser>)_userStore;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
      _context = context;
      _accountService = accountService;
    }

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

    private string GetErrors(IEnumerable<string> errors)
    {
      var sb = new StringBuilder();

      foreach(var error in errors)
      {
        sb.Append(error);
        sb.Append("; ");
      }

      return sb.ToString();
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
    public async Task<IActionResult> Registration([FromForm] RegistrationModel model)
    {
      var methodName = nameof(Registration);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = GetErrors(_accountService.GetModelErrors(ModelState));

        Logger(LogLevel.Warning, methodName, $"Model invalid. Errors: {modelStateErrors}");
        return View();
      }

      if(!await _accountService.CheckIsEmailExistAsync(model.Email!))
      {
        ModelState.AddModelError(nameof(model.Email), $"Электронная почта уже используется");
        Logger(LogLevel.Warning, methodName, $"Email already used.");
        return View();
      }

      var user = new AppUser
      {
        UserName = model.Email,
        Email = model.Email,
        Name = model.Name,
      };

      await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
      await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
      var resultOfCreatingUser = await _userManager.CreateAsync(user, model.Password!);

      if(resultOfCreatingUser.Succeeded)
      {
        Logger(LogLevel.Information, methodName, "User is created", user.Id);

        var roleClaim = new Claim(ClaimTypes.Role, RoleClaims.User.ToString());
        await _userManager.AddClaimAsync(user, roleClaim);

        var userId = await _userManager.GetUserIdAsync(user);
        var token = await _accountService.GenerateEmailConfirmationTokenAsync(user);

        var callBackUrl = Url.Action(
            action: nameof(RegistrationConfirmation),
            controller: typeof(AccountController).ControllerName(),
            values: new { userId, token, statusConfirmation = true },
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(model.Email!, "Подтверждение регистрации",
            $"Спасибо за регистрацию. Пожалуйста, перейдите по ссылке , чтобы подтвердить ваш адрес электронной почты: <a href='{HtmlEncoder.Default.Encode(callBackUrl!)}' id='confrimationLink'>нажмите сюда</a>." +
            $"\n\nЕсли вы получили это письмо случайно - удалите это письмо.");
        ViewBag.StatusMessage = $"Спасибо за регистрацию. На ваш электронный адрес выслано письмо с подтверждением регистрации.";

        Logger(LogLevel.Information, methodName, "Email with registration confirmation has sent", userId);

        return View("_StatusMessage");
      }

      var creatingUserErrors = GetErrors(_accountService.GetIdentityResultErrors(resultOfCreatingUser));
      Logger(LogLevel.Error, methodName, $"User not created, Errors: {creatingUserErrors}", user.Id);
      return View();
    }

    /// <summary>
    /// Обрабатывает подтверждение регистрации пользователя по электронной почте.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="token">Код подтверждения из электронной почты.</param>
    /// <returns>
    /// Возвращает страницу с сообщением об успешном завершении регистрации или сообщение об ошибке.
    /// </returns>
    public async Task<IActionResult> RegistrationConfirmation([FromQuery] string userId, [FromQuery] string token)
    {
      var methodName = nameof(RegistrationConfirmation);

      if(userId == null)
      {
        Logger(LogLevel.Error, methodName, "UserId is null");
        return View("_StatusMessage", "Ошибка! Пользователь не найден!");
      }
      else if(token == null)
      {
        Logger(LogLevel.Error, methodName, "Verification token is null");
        return View("_StatusMessage", "Ошибка! Код подтверждения не найден!");
      }

      var user = await _userManager.FindByIdAsync(userId);

      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User has not found", userId);
        return View("_StatusMessage", "Ошибка. Пользователь не найден или истек срок годности кода подтверждения");
      }

      token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

      var resultOfConfrimEmail = await _userManager.ConfirmEmailAsync(user, token);
      if(resultOfConfrimEmail.Succeeded)
      {
        Logger(LogLevel.Information, methodName, "User is registered", userId);
        ViewBag.StatusMessage = "Регистрация завершена.";
      }
      else
      {
        var errors = GetErrors(_accountService.GetIdentityResultErrors(resultOfConfrimEmail));

        Logger(LogLevel.Warning, methodName, $"User not registered. Errors: {errors}", userId);
        ViewBag.StatusMessage = "Ошибка при подтверждении эл.почты! Обратитесь к администрации.";
      }

      return View("_StatusMessage");
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
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
      var methodName = nameof(Login);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = GetErrors(_accountService.GetModelErrors(ModelState));
        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {modelStateErrors}");

        return View();
      }

      var resultOfSignIn = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

      if(resultOfSignIn.Succeeded)
      {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        // Создание ClaimsPrincipal на основе пользователя
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        // Проверяем явлется ли это первым входом для пользователя с ролью Admin
        if(user!.IsFirstLogin && userPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
        {
          //Генерируем токен изменения пароля и перенаправляем на страницу изменения пароля
          var token = await _accountService.GeneratePasswordResetTokenAsync(user);
          return RedirectToAction(nameof(ResetPassword), new { token, user.Email });
        }
        // Установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
        Logger(LogLevel.Information, methodName, "User signed in", user.Id);
        return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
      }

      Logger(LogLevel.Warning, methodName, "Login or password is incorrect.");
      ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
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
      var methodName = nameof(Logout);
      var userId = User.Claims
        .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      await _signInManager.SignOutAsync();

      Logger(LogLevel.Information, methodName, "User logged out", userId.Value);
      return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
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
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordModel model)
    {
      var methodName = nameof(ForgotPassword);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = GetErrors(_accountService.GetModelErrors(ModelState));

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {modelStateErrors}");
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
      {
        Logger(LogLevel.Error, methodName, "User not exist");
        ModelState.AddModelError(nameof(model.Email), "Пользователя с такой электронной почтой не существует.");
        return View();
      }

      var token = await _accountService.GeneratePasswordResetTokenAsync(user);

      var callBackUrl = Url.Action(
          action: nameof(ResetPassword),
          controller: typeof(AccountController).ControllerName(),
          values: new { token, model.Email },
          protocol: Request.Scheme);

      await _emailSender.SendEmailAsync(model.Email!, "Восстановление пароля",
          $"Для восстановления пароля перейдите по ссылке : <a href = '{HtmlEncoder.Default.Encode(callBackUrl!)}'>нажмите сюда</a>.");

      Logger(LogLevel.Information, methodName, "Email for reset password has sent", user.Id);
      return View("_StatusMessage", "Проверьте вашу электронную почту, чтобы восстановить пароль.");
    }

    /// <summary>
    /// Отображает страницу сброса пароля.
    /// </summary>
    /// <param name="token">Код сброса пароля.</param>
    /// <param name="email">Адрес электронной почты пользователя.</param>
    /// <returns>
    /// Возвращает страницу сброса пароля или ошибку BadRequest.
    /// </returns>
    [HttpGet]
    public IActionResult ResetPassword(string? token = null, string? email = null)
    {
      var methodName = nameof(ResetPassword);

      if(String.IsNullOrEmpty(token))
      {
        Logger(LogLevel.Error, methodName, "Verification token is null");
        return View("_StatusMessage", "Ошибка! Код сброса пароля не найден.");
      }
      if(String.IsNullOrEmpty(email))
      {
        Logger(LogLevel.Error, methodName, "Email is null");
        return View("_StatusMessage", "Ошибка! Электронная почта не найдена.");
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
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
    {
      var methodName = nameof(ResetPassword);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = GetErrors(_accountService.GetModelErrors(ModelState));
        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {modelStateErrors}");

        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return View("_StatusMessage", "Ошибка во время сброса пароля. Попробуйте ещё раз.");
      }

      model.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token!));
      var resultOfResetPassword = await _userManager.ResetPasswordAsync(user, model.Token, model.Password!);

      if(resultOfResetPassword.Succeeded)
      {
        Logger(LogLevel.Information, methodName, "Password is reset", user.Id);
        // Создание ClaimsPrincipal на основе пользователя
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        // Проверяем явлется ли это первым входом для пользователя с ролью Admin, изменяем статус флага и обновляем значение в БД
        if(user.IsFirstLogin && userPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
        {
          user.IsFirstLogin = false;
          _context.Update(user);
          await _context.SaveChangesAsync();
        }

        Logger(LogLevel.Information, methodName, "Password is changed", user.Id);
        return View("_StatusMessage", $"Пароль изменен.");
      }

      foreach(var error in resultOfResetPassword.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      var errors = GetErrors(_accountService.GetIdentityResultErrors(resultOfResetPassword));
      Logger(LogLevel.Warning, methodName, $"Reset password has failed. Errors: {errors}");

      return View();
    }
  }
}