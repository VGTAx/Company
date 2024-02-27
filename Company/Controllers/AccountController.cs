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
    private readonly AccountServiceBase _accountService;
    private readonly ICompanyContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IUserStore<ApplicationUserModel> _userStore;
    private readonly IUserEmailStore<ApplicationUserModel> _emailStore;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<ApplicationUserModel> _signInManager;
    private readonly UserManager<ApplicationUserModel> _userManager;

    /// <summary>
    /// Создает экземпляр класса <see cref="AccountController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей для работы с учетными записями.</param>
    /// <param name="userStore">Хранилище пользователей.</param>
    /// <param name="signInManager">Менеджер аутентификации и входа в систему.</param>
    /// <param name="emailSender">Сервис отправки электронных писем.</param>
    /// <param name="roleManager">Менеджер ролей для работы с ролями пользователей.</param>    
    public AccountController(
        AccountServiceBase accountService,
        ICompanyContext context,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        IUserStore<ApplicationUserModel> userStore,
        UserManager<ApplicationUserModel> userManager,
        SignInManager<ApplicationUserModel> signInManager
        )
    {
      _userManager = userManager;
      _userStore = userStore;
      _emailStore = (IUserEmailStore<ApplicationUserModel>)_userStore;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
      _context = context;
      _accountService = accountService;
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _accountService.GetModelStateErrors(ModelState);

        _logger.LogInformation("Registration has failed. Model isn't valid. Errors: {error}", modelStateErrors);
        return View();
      }

      if(!await _accountService.CheckIsEmailExistAsync(model.Email!))
      {
        ModelState.AddModelError(nameof(model.Email), $"Электронная почта уже используется");
        _logger.LogWarning("Registration has failed. Email has already used.");
        return View();
      }

      var user = new ApplicationUserModel
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
        _logger.LogInformation("User {user} is created at {CurrentTime}", user.Id, DateTime.Now);

        var roleClaim = new Claim(ClaimTypes.Role, "User");
        await _userManager.AddClaimAsync(user, roleClaim);

        var userId = await _userManager.GetUserIdAsync(user);
        var token = await _accountService.GenereateEmailConfirmationTokenAsync(user);

        var callBackUrl = Url.Action(
            action: nameof(RegistrationConfirmation),
            controller: typeof(AccountController).ControllerName(),
            values: new { userId, token, statusConfirmation = true },
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(model.Email!, "Подтверждение регистрации",
            $"Спасибо за регистрацию. Пожалуйста, перейдите по ссылке , чтобы подтвердить ваш адрес электронной почты: <a href='{HtmlEncoder.Default.Encode(callBackUrl!)}' id='confrimationLink'>нажмите сюда</a>." +
            $"\n\nЕсли вы получили это письмо случайно - удалите это письмо.");
        ViewBag.StatusMessage = $"Спасибо за регистрацию. На ваш электронный адрес выслано письмо с подтверждением регистрации.";

        _logger.LogInformation("Email with registration confirmation has sent");

        return View("_StatusMessage");
      }

      var creatingUserErrors = _accountService.GetIdentityResultErrors(resultOfCreatingUser);

      _logger.LogWarning("Registration has failed. User {user} hasn't created. Errors: {errors}", user.Id, creatingUserErrors);
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
      if(userId == null)
      {
        _logger.LogWarning("Registration confirmation has failed. UserId is null");
        return View("_StatusMessage", "Ошибка! Пользователь не найден!");
      }
      else if(token == null)
      {
        _logger.LogWarning("Registration confirmation has failed. Verification token is null");
        return View("_StatusMessage", "Ошибка! Код подтверждения не найден!");
      }

      var user = await _userManager.FindByIdAsync(userId);

      if(user == null)
      {
        _logger.LogWarning("Registration confirmation has failed. User {userId} has not found", userId);
        return View("_StatusMessage", "Ошибка. Пользователь не найден или истек срок годности кода подтверждения");
      }

      token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

      var resultOfConfrimEmail = await _userManager.ConfirmEmailAsync(user, token);
      if(resultOfConfrimEmail.Succeeded)
      {
        _logger.LogInformation("User {user} is registered", userId);
        ViewBag.StatusMessage = "Регистрация завершена.";
      }
      else
      {
        var errors = _accountService.GetIdentityResultErrors(resultOfConfrimEmail);

        _logger.LogWarning("Registration confirmation has failed. User {user} hasn't registered. Errors: {errors}", userId, errors);
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _accountService.GetModelStateErrors(ModelState);
        _logger.LogInformation("Login is failed. Model isn't valid. Errors: {error}", modelStateErrors);
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
          var token = await _accountService.GenereatePasswordResetTokenAsync(user);
          return RedirectToAction(nameof(ResetPassword), new { token, user.Email });
        }
        // Установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
        _logger.LogInformation("User {user} is logged in", user.Id);
        return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
      }

      _logger.LogWarning("Login is failed. Login or password is incorrect.");
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
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      _logger.LogInformation("Logout method. User has logged out");
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _accountService.GetModelStateErrors(ModelState);

        _logger.LogInformation("Reset password token has not created. Model isn't valid. Errors: {error}", modelStateErrors);
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
      {
        _logger.LogWarning("Reset password token has not created. User doesn't exist.");
        ModelState.AddModelError(nameof(model.Email), "Пользователя с такой электронной почтой не существует.");
        return View();
      }

      var token = await _accountService.GenereatePasswordResetTokenAsync(user);

      var callBackUrl = Url.Action(
          action: nameof(ResetPassword),
          controller: typeof(AccountController).ControllerName(),
          values: new { token, model.Email },
          protocol: Request.Scheme);

      await _emailSender.SendEmailAsync(model.Email!, "Восстановление пароля",
          $"Для восстановления пароля перейдите по ссылке : <a href = '{HtmlEncoder.Default.Encode(callBackUrl!)}'>нажмите сюда</a>.");

      _logger.LogInformation("Reset password token has created. Email for reset password has sent");
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
      if(String.IsNullOrEmpty(token))
      {
        _logger.LogWarning("Reset password failed. Verification token is null");
        return View("_StatusMessage", "Ошибка! Код сброса пароля не найден.");
      }
      if(String.IsNullOrEmpty(email))
      {
        _logger.LogWarning("Reset password failed. Email is null");
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _accountService.GetModelStateErrors(ModelState);

        _logger.LogInformation("Reset password failed. Model isn't valid. Errors: {error}", modelStateErrors);
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null)
      {
        _logger.LogWarning("Reset password failed. User has not found", model.Email);
        return View("_StatusMessage", "Ошибка во время сброса пароля. Попробуйте ещё раз.");
      }

      model.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token!));
      var resultOfResetPassword = await _userManager.ResetPasswordAsync(user, model.Token, model.Password!);

      if(resultOfResetPassword.Succeeded)
      {
        _logger.LogInformation("Password for user {user} is reset", user.Id);
        // Создание ClaimsPrincipal на основе пользователя
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        // Проверяем явлется ли это первым входом для пользователя с ролью Admin, изменяем статус флага и обновляем значение в БД
        if(user.IsFirstLogin && userPrincipal.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
        {
          user.IsFirstLogin = false;
          _context.Update(user);
          await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Password for user {user} has changed", user.Id);
        return View("_StatusMessage", $"Пароль изменен.");
      }

      foreach(var error in resultOfResetPassword.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      var errors = _accountService.GetIdentityResultErrors(resultOfResetPassword);
      _logger.LogWarning("Reset password has failed.Errors: {errors}", errors);

      return View();
    }
  }
}