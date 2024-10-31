using Company.BaseClass;
using Company.Models;
using Company.Models.ManageAccount;
using Company.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace Company.Controllers
{
  /// <summary>
  /// Контроллер для управления учетной записью.
  /// </summary>
  [Authorize(Policy = "BasicPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class ManageAccountController : Controller
  {
    private readonly ManageAccountBase<AppUser> _manageAccountService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly StringParser _stringParser;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ManageAccountController> _logger;

    /// <summary>
    /// Создает экземпляр класса <see cref="ManageAccountController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей.</param>
    /// <param name="signInManager">Менеджер аутентификации.</param>
    /// <param name="emailSender">Сервис отправки электронной почты.</param>
    public ManageAccountController(
        ManageAccountBase<AppUser> manageAccountService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        StringParser stringParser,
        ILogger<ManageAccountController> logger,
        IEmailSender emailSender)
    {
      _manageAccountService = manageAccountService;
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _stringParser = stringParser;
      _logger = logger;
    }

    private void Logger(LogLevel logLevel, string methodName, string message, string? employeeId = default)
    {
      var ip = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
      _logger.Log(logLevel, ": {IP} {method} - {employeeId} {message}", ip, methodName, employeeId, message);
    }

    /// <summary>
    /// Метод возвращает список функций для управления аккаунтом в виде частичного представления.
    /// </summary>
    /// <returns>Partial View, содержащее список функций для управления аккаунтом.</returns>
    public IActionResult Index()
    {
      return View();
    }

    /// <summary>
    /// Метод отображает страницу профиля пользователя.
    /// </summary>
    /// <returns>Partial View с профилем пользователя.</returns>
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
      var methodName = nameof(Profile);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      var model = new ProfileModel
      {
        Email = user.Email,
        Name = user.Name,
        Phone = user.PhoneNumber,
      };

      Logger(LogLevel.Information, methodName, "Get user profile", user.Id);

      ViewBag.ActiveLink = "profile";
      return PartialView(model);
    }

    /// <summary>
    /// Обновление профиля пользователя на основе данных из модели.
    /// </summary>
    /// <param name="model">Модель с данными профиля пользователя.</param>
    /// <returns>Partial View с информацией о статусе обновления профиля.</returns>
    [HttpPost]
    public async Task<IActionResult> Profile([FromForm] ProfileModel model)
    {
      var methodName = nameof(Profile);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
      }

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {errors}", user.Id);
        return BadRequest(ModelState);
      }

      var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

      if(model.Phone != phoneNumber)
      {
        var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
        if(!setPhoneNumberResult.Succeeded)
        {
          var setPhoneNumberResultErrors = _manageAccountService.GetIdentityResultErrors(setPhoneNumberResult);
          var errors = _stringParser.CollectionToString(setPhoneNumberResultErrors);

          Logger(LogLevel.Warning, methodName, $"Change phone number failed. Errors: {errors}", user.Id);
          return PartialView(model);
        }

        Logger(LogLevel.Information, methodName, "Phone number changed", user.Id);
        ViewBag.StatusMessage = "Профиль изменен"!;
      }

      if(user.Name != model.Name)
      {
        user.Name = model.Name;
        var updateNameResult = await _userManager.UpdateAsync(user);

        if(!updateNameResult.Succeeded)
        {
          var setPhoneNumberResultErrors = _manageAccountService.GetIdentityResultErrors(updateNameResult);
          var errors = _stringParser.CollectionToString(setPhoneNumberResultErrors);

          Logger(LogLevel.Warning, methodName, $"Change name failed. Errors: {errors}", user.Id);
          return PartialView(model);
        }

        Logger(LogLevel.Information, methodName, "Name changed", user.Id);
        ViewBag.StatusMessage = "Профиль изменен"!;
      }

      if(ViewBag.StatusMessage == "Профиль изменен"!)
      {
        await _signInManager.RefreshSignInAsync(user);
        Logger(LogLevel.Information, methodName, "User resign in", user.Id);

        return PartialView("_StatusMessage", ViewBag.StatusMessage);
      }
      else
      {
        return PartialView(model);
      }
    }

    /// <summary>
    /// Метод отображает страницу с формой изменения электронной почты пользователя.
    /// </summary>
    /// <returns>Partial View с формой изменения электронной почты.</returns>
    [HttpGet]
    public async Task<IActionResult> ChangeEmail()
    {
      var methodName = nameof(ChangeEmail);

      var user = await _userManager.GetUserAsync(User);

      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      var email = await _userManager.GetEmailAsync(user);
      var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

      var model = new ChangeEmailModel
      {
        Email = email,
        IsEmailConfirmed = isEmailConfirmed,
      };

      Logger(LogLevel.Information, methodName, "Get change email menu");
      return PartialView(model);
    }
    /// <summary>
    /// Обработка POST-запроса при изменении электронной почты пользователя.
    /// </summary>
    /// <param name="model">Модель данных с новой электронной почтой и флагом подтверждения электронной почты.</param>
    /// <returns>Partial View с результатом операции изменения электронной почты пользователя.</returns>
    [HttpPost]
    public async Task<IActionResult> ChangeEmail([Bind("NewEmail, Email")] ChangeEmailModel model)
    {
      var methodName = nameof(ChangeEmail);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {errors}", user.Id);
        return PartialView();
      }

      model.Email = await _userManager.GetEmailAsync(user);

      var userId = await _userManager.GetUserIdAsync(user);
      var checkAvailableEmail = await _userManager.FindByEmailAsync(model.NewEmail!);

      if(model.NewEmail != model.Email && checkAvailableEmail is null)
      {
        var token = await _manageAccountService.GenerateChangeEmailTokenAsync(user, model.NewEmail!);
        Logger(LogLevel.Information, methodName, $"Change email token is generated", user.Id);

        var callbackUrl = Url.Action(
            action: nameof(ChangeEmailConfirmation),
            controller: typeof(ManageAccountController).ControllerName(),
            values: new { userId, email = model.NewEmail, token },
            protocol: Request.Scheme);

        var message = $"Добрый день. Подтвердите изменение эл.почты <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>нажмите сюда</a>";
        await _emailSender.SendEmailAsync(model.NewEmail!, "Подтверждение изменения электронной почты", message);

        Logger(LogLevel.Information, methodName, $"Email with change email confirmation", user.Id);
        return PartialView("_StatusMessage", "Пожалуйста, проверьте электронную почту, чтобы подтвердить изменения");
      }

      ModelState.AddModelError("NewEmail", "Электронная почта уже используется");
      Logger(LogLevel.Warning, methodName, "Email has already used", user.Id);

      return PartialView(model);
    }

    /// <summary>
    /// Обработка GET-запроса подтверждения изменения электронной почты пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="email">Новая электронная почта пользователя.</param>
    /// <param name="token">Код подтверждения изменения электронной почты.</param>
    /// <returns>View с результатом операции подтверждения изменения электронной почты пользователя.</returns>
    public async Task<IActionResult> ChangeEmailConfirmation(string userId, string email, string token)
    {
      var methodName = nameof(ChangeEmailConfirmation);

      if(userId == null)
      {
        Logger(LogLevel.Error, methodName, "User Id is null");
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }
      if(token is null)
      {
        Logger(LogLevel.Error, methodName, "Verification token is null", userId);
        _logger.LogWarning("Confirmation new email has failed. Verification token is null");
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }
      if(email == null)
      {
        Logger(LogLevel.Error, methodName, "Email is null", userId);
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found", userId);
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты. Пользователь не найден.");
      }

      token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

      var resultChangeEmail = await _userManager.ChangeEmailAsync(user, email, token);

      if(!resultChangeEmail.Succeeded)
      {
        var resultChangeEmailErrors = _manageAccountService.GetIdentityResultErrors(resultChangeEmail);
        var errors = _stringParser.CollectionToString(resultChangeEmailErrors);

        Logger(LogLevel.Warning, methodName, $"Confirmation new email has failed. Errors: {errors}", userId);
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }

      await _userManager.SetUserNameAsync(user, email);
      Logger(LogLevel.Information, methodName, "Confirm new email", user.Id);

      await _signInManager.RefreshSignInAsync(user);
      Logger(LogLevel.Information, methodName, "User resign in", user.Id);

      return View("_StatusMessage", "Электронная почта изменена");
    }

    /// <summary>
    /// Метод отображает страницу с формой изменения пароля пользователя (GET-запрос).
    /// </summary>
    /// <returns>View страницы изменения пароля.</returns>
    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
      var methodName = nameof(ChangePassword);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      Logger(LogLevel.Information, methodName, "Get ChangePassword menu", user.Id);
      return PartialView();
    }
    /// <summary>
    /// Изменение пароля пользователя на основе данных из формы (POST-запрос).
    /// </summary>
    /// <param name="model">Модель данных для изменения пароля.</param>
    /// <returns>Partial View с результатом выполнения операции.</returns>
    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel model)
    {
      var methodName = nameof(ChangePassword);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Information, methodName, $"Model isn't valid. Errors: {errors}");
        return BadRequest(ModelState);
      }

      var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);

      if(!changePasswordResult.Succeeded)
      {
        var resultChangePasswordErrors = _manageAccountService.GetIdentityResultErrors(changePasswordResult);
        var errors = _stringParser.CollectionToString(resultChangePasswordErrors);

        Logger(LogLevel.Warning, methodName, $"Change password failed. Errors: {errors}", user.Id);
        ModelState.AddModelError(string.Empty, "Неверный старый пароль.");
        return PartialView();
      }

      Logger(LogLevel.Information, methodName, "Password changed", user.Id);

      await _signInManager.RefreshSignInAsync(user);
      Logger(LogLevel.Information, methodName, "User resign in", user.Id);

      return PartialView("_StatusMessage", "Пароль изменен!");
    }

    /// <summary>
    /// Метод отображает страницу с персональными данными пользователя (GET-запрос).
    /// </summary>
    /// <returns>View с персональными данными пользователя.</returns>
    [HttpGet]
    public IActionResult PersonalData()
    {
      return PartialView();
    }

    /// <summary>
    /// Метод отображает страницу для удаления персональных данных пользователя (GET-запрос).
    /// </summary>
    /// <returns>View для удаления персональных данных пользователя.</returns>
    [HttpGet]
    public async Task<IActionResult> DeletePersonalData()
    {
      var methodName = nameof(DeletePersonalData);

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      var model = new DeletePersonalDataModel
      {
        RequirePassword = await _userManager.HasPasswordAsync(user!),
        Password = "",
      };

      Logger(LogLevel.Information, methodName, "Get DeletePersonalData menu", user.Id);
      return PartialView(model);
    }

    /// <summary>
    /// Метод выполняет удаление персональных данных пользователя (POST-запрос).
    /// </summary>
    /// <param name="model">Модель, содержащая информацию для удаления персональных данных.</param>
    /// <returns>Результат операции удаления персональных данных.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    [HttpPost]
    public async Task<IActionResult> DeletePersonalData([FromForm] DeletePersonalDataModel model)
    {
      var methodName = nameof(DeletePersonalData);

      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        var errors = _stringParser.CollectionToString(modelStateErrors);

        Logger(LogLevel.Warning, methodName, $"Model isn't valid. Errors: {errors}");
        return BadRequest(ModelState);
      }

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        Logger(LogLevel.Error, methodName, "User not found");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      model.RequirePassword = await _userManager.HasPasswordAsync(user!);

      if(model.RequirePassword && !await _userManager.CheckPasswordAsync(user!, model.Password!))
      {
        Logger(LogLevel.Warning, methodName, "Incorrect password", user.Id);
        ModelState.AddModelError("Password", "Неверный пароль");

        return BadRequest(ModelState);
      }

      var resultDeletePersonalData = await _userManager.DeleteAsync(user!);
      if(!resultDeletePersonalData.Succeeded)
      {
        var resultDeletePersonalDataErrors = _manageAccountService.GetIdentityResultErrors(resultDeletePersonalData);
        var errors = _stringParser.CollectionToString(resultDeletePersonalDataErrors);

        Logger(LogLevel.Warning, methodName, $"Delete personal data failed. Errors: {errors}", user.Id);
        return PartialView("_StatusMessage", "Ошибка при удалении пользователя!");
      }

      Logger(LogLevel.Information, methodName, "Personal data deleted", user.Id);

      await _signInManager.SignOutAsync();
      Logger(LogLevel.Information, methodName, "User sign out", user.Id);

      return Ok();
    }
  }
}