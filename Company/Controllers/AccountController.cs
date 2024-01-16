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
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;

    /// <summary>
    /// Создает экземпляр класса <see cref="AccountController"/>.
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
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManger
        )
    {
      _userManager = userManager;
      _userStore = userStore;
      _emailStore = (IUserEmailStore<ApplicationUserModel>)_userStore;
      _signInManager = signInManager;
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
    public async Task<IActionResult> Registration([FromForm] RegistrationModel model)
    {
      if(!ModelState.IsValid)
      {
        return View();
      }

      var checkExistEmail = await _userManager.FindByEmailAsync(model.Email!);

      if(checkExistEmail != null)
      {
        ModelState.AddModelError(nameof(model.Email), $"Электронная почта уже используется");
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
        var role = await _roleManager.FindByNameAsync("User") ?? throw new ArgumentException("Role \"User\" not found");
        var roleClaim = new Claim(ClaimTypes.Role, role.Name!);
        await _userManager.AddClaimAsync(user, roleClaim);

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callBackUrl = Url.Action(
            action: nameof(RegistrationConfirmation),
            controller: typeof(AccountController).ControllerName(),
            values: new { userId, code, statusConfirmation = true },
            protocol: Request.Scheme);

        await _userManager.AddToRoleAsync(user, "User");

        await _emailSender.SendEmailAsync(model.Email!, "Подтверждение регистрации",
            $"Спасибо за регистрацию. Пожалуйста, перейдите по ссылке , чтобы подтвердить ваш адрес электронной почты: <a href='{HtmlEncoder.Default.Encode(callBackUrl!)}' id='confrimationLink'>нажмите сюда</a>." +
            $"\n\nЕсли вы получили это письмо случайно - удалите это письмо.");

        ViewBag.StatusMessage = $"Спасибо за регистрацию. На ваш электронный адрес выслано письмо с подвтерждением регистрации.";
        return View("_StatusMessage");
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
    public async Task<IActionResult> RegistrationConfirmation([FromQuery] string userId, [FromQuery] string code)
    {
      if(userId == null || code == null)
      {
        return View("_StatusMessage", "Ошибка! Что-то пошло не так..");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if(user == null)
      {
        return View("_StatusMessage", "Ошибка. Пользователь не найден или истек срок годности кода подтверждения");
      }

      code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

      var result = await _userManager.ConfirmEmailAsync(user, code);
      ViewBag.StatusMessage = result.Succeeded ? "Регистрация завершена." : "Ошибка при подтверждении эл.почты";

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
        return View();
      }

      var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

      if(result.Succeeded)
      {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        // Создание ClaimsPrincipal на основе пользователя
        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user!);
        // Установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

        return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
        return View();
      }
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
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
      {
        ModelState.AddModelError(nameof(model.Email), "Пользователя с такой электронной почтой не существует.");
        return View();
      }

      var code = await _userManager.GeneratePasswordResetTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      var callBackUrl = Url.Action(
          action: nameof(ResetPassword),
          controller: typeof(AccountController).ControllerName(),
          values: new { code, model.Email },
          protocol: Request.Scheme);
      await _emailSender.SendEmailAsync(model.Email!, "Восстановление пароля",
          $"Для восстановления пароля перейдите по ссылке : <a href = '{HtmlEncoder.Default.Encode(callBackUrl!)}'>нажмите сюда</a>.");

      return View("_StatusMessage", "Проверьте вашу электронную почту, чтобы восстановить пароль.");
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
    public IActionResult ResetPassword(string? code = null, string? email = null)
    {
      if(code == null || email == null)
      {
        return View("_StatusMessage", "Ошибка! Неверный код подтверждения.");
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
        return View();
      }

      var user = await _userManager.FindByEmailAsync(model.Email!);
      if(user == null)
      {
        return View("_StatusMessage", "Ошибка во время сброса пароля. Попробуйте ещё раз.");
      }

      model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code!));
      var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password!);

      if(result.Succeeded)
      {
        return View("_StatusMessage", $"Пароль изменен.");
      }

      foreach(var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View();
    }
  }
}