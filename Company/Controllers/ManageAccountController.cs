using Company.BaseClass;
using Company.Models;
using Company.Models.ManageAccount;
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
    private readonly ManageAccountBase<ApplicationUserModel> _manageAccountService;
    private readonly UserManager<ApplicationUserModel> _userManager;
    private readonly SignInManager<ApplicationUserModel> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ManageAccountController> _logger;
    /// <summary>
    /// Создает экземпляр класса <see cref="ManageAccountController"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей.</param>
    /// <param name="signInManager">Менеджер аутентификации.</param>
    /// <param name="emailSender">Сервис отправки электронной почты.</param>
    public ManageAccountController(
        ManageAccountBase<ApplicationUserModel> manageAccountService,
        UserManager<ApplicationUserModel> userManager,
        SignInManager<ApplicationUserModel> signInManager,
        ILogger<ManageAccountController> logger,
        IEmailSender emailSender)
    {
      _manageAccountService = manageAccountService;
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
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
      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("User profile is not loaded. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      var model = new ProfileModel
      {
        Email = user.Email,
        Name = user.Name,
        Phone = user.PhoneNumber,
      };
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        _logger.LogInformation("Update profile has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return BadRequest(ModelState);
      }

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("Update profile of user has failed. User not found");
        return RedirectToAction(nameof(DepartmentController.Index), typeof(DepartmentController).ControllerName());
      }

      var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

      if(model.Phone != phoneNumber)
      {
        var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
        if(!setPhoneNumberResult.Succeeded)
        {
          var setPhoneNumberResultErrors = _manageAccountService.GetIdentityResultErrors(setPhoneNumberResult);

          _logger.LogWarning("Update profile of user {id} has failed. Change phone number is failed." +
            "Errors: {errors}", user.Id, setPhoneNumberResultErrors);
          return PartialView(model);
        }
        _logger.LogInformation("Update profile of user {id} has succeeded. Phone number is changed.", user.Id);
        ViewBag.StatusMessage = "Профиль изменен"!;
      }

      if(user.Name != model.Name)
      {
        user.Name = model.Name;
        var updateNameResult = await _userManager.UpdateAsync(user);

        if(!updateNameResult.Succeeded)
        {
          var setPhoneNumberResultErrors = _manageAccountService.GetIdentityResultErrors(updateNameResult);
          _logger.LogWarning("Update profile of user {id} has failed. Change name is failed." +
            "Errors: {errors}", user.Id, setPhoneNumberResultErrors);

          return PartialView(model);
        }

        _logger.LogInformation("Update profile of user {id} has succeeded. Name is changed.", user.Id);
        ViewBag.StatusMessage = "Профиль изменен"!;
      }

      if(ViewBag.StatusMessage == "Профиль изменен"!)
      {
        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation("Update profile of user {id} has succeeded. User re sign in", user.Id);

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
      var user = await _userManager.GetUserAsync(User);

      if(user == null)
      {
        _logger.LogWarning("Change email menu is not loaded. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      var email = await _userManager.GetEmailAsync(user);
      var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

      var model = new ChangeEmailModel
      {
        Email = email,
        IsEmailConfirmed = isEmailConfirmed,
      };
      _logger.LogInformation("Change email menu is loaded");
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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        _logger.LogInformation("Change email has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return PartialView();
      }

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("Change email has failed. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден.");
      }

      model.Email = await _userManager.GetEmailAsync(user);

      var userId = await _userManager.GetUserIdAsync(user);
      var checkAvailableEmail = await _userManager.FindByEmailAsync(model.NewEmail!);

      if(model.NewEmail != model.Email && checkAvailableEmail is null)
      {
        var token = await _manageAccountService.GenerateChangeEmailTokenAsync(user, model.NewEmail!);
        _logger.LogInformation("Change email method. Change email token is generated");

        var callbackUrl = Url.Action(
            action: nameof(ChangeEmailConfirmation),
            controller: typeof(ManageAccountController).ControllerName(),
            values: new { userId, email = model.NewEmail, token },
            protocol: Request.Scheme);

        var message = $"Добрый день. Подтвердите изменение эл.почты <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>нажмите сюда</a>";
        await _emailSender.SendEmailAsync(model.NewEmail!, "Подтверждение изменения электронной почты", message);

        _logger.LogInformation("Change email has succeeded. Email with  change email confirmation has sent.");
        return PartialView("_StatusMessage", "Пожалуйста, проверьте электронную почту, чтобы подтвердить изменения");
      }

      ModelState.AddModelError("NewEmail", "Электронная почта уже используется");
      _logger.LogWarning("Change email has failed. Email has already used");

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
      if(userId == null)
      {
        _logger.LogWarning("Confirmation new email has failed. User ID is null");
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }
      if(token is null)
      {
        _logger.LogWarning("Confirmation new email has failed. Verification token is null");
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }
      if(email == null)
      {
        _logger.LogWarning("Confirmation new email has failed. User email is null");
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if(user == null)
      {
        _logger.LogWarning("Confirmation new email has failed. User {id} not found", userId);
        return View("_StatusMessage", "Ошибка при подтверждении электронной почты. Пользователь не найден.");
      }

      token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

      var resultChangeEmail = await _userManager.ChangeEmailAsync(user, email, token);

      if(!resultChangeEmail.Succeeded)
      {
        var resultChangeEmailErrors = _manageAccountService.GetIdentityResultErrors(resultChangeEmail);
        _logger.LogWarning("Confirmation new email has failed. User {id}. Errors: {errors}", user.Id, resultChangeEmailErrors);

        return View("_StatusMessage", "Ошибка при подтверждении электронной почты.");
      }

      await _userManager.SetUserNameAsync(user, email);
      _logger.LogInformation("Confirmation new email has succeeded. User {id}.", user.Id);

      await _signInManager.RefreshSignInAsync(user);
      _logger.LogInformation("Confirmation new email. User {id} re sign in.", user.Id);

      return View("_StatusMessage", "Электронная почта изменена");
    }

    /// <summary>
    /// Метод отображает страницу с формой изменения пароля пользователя (GET-запрос).
    /// </summary>
    /// <returns>View страницы изменения пароля.</returns>
    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("ChangePassword menu has not gotten. User (Principal) is null");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        _logger.LogInformation("Change password has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return BadRequest(ModelState);
      }

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("Change password has failed. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);

      if(!changePasswordResult.Succeeded)
      {
        var resultChangePasswordErrors = _manageAccountService.GetIdentityResultErrors(changePasswordResult);
        _logger.LogWarning("Change password has failed. User {id}. Errors: {errors}", user.Id, resultChangePasswordErrors);

        ModelState.AddModelError(string.Empty, "Неверный старый пароль.");
        return PartialView();
      }

      _logger.LogInformation("Change password has succeeded. User {id}.", user.Id);

      await _signInManager.RefreshSignInAsync(user);
      _logger.LogInformation("Change password. User {id}. Refresh sign in.", user.Id);

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
      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("Delete personal data menu has not gotten. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      var model = new DeletePersonalDataModel
      {
        RequirePassword = await _userManager.HasPasswordAsync(user!),
        Password = "",
      };

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
      if(!ModelState.IsValid)
      {
        var modelStateErrors = _manageAccountService.GetModelErrors(ModelState);
        _logger.LogInformation("Delete personal data has failed. Model isn't valid. Errors: {errors}", modelStateErrors);

        return BadRequest(ModelState);
      }

      var user = await _userManager.GetUserAsync(User);
      if(user == null)
      {
        _logger.LogWarning("Delete personal data has failed. User (Principal) is null!");
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      model.RequirePassword = await _userManager.HasPasswordAsync(user!);

      if(model.RequirePassword && !await _userManager.CheckPasswordAsync(user!, model.Password!))
      {
        ModelState.AddModelError("Password", "Неверный пароль");
        _logger.LogWarning("Delete personal data has failed. Incorrect password");

        return BadRequest(ModelState);
      }

      var resultDeletePersonalData = await _userManager.DeleteAsync(user!);
      if(!resultDeletePersonalData.Succeeded)
      {
        var resultDeletePersonalDataErrors = _manageAccountService.GetIdentityResultErrors(resultDeletePersonalData);
        _logger.LogWarning("Delete personal data has failed. User {id}. Errors: {errors}", user.Id, resultDeletePersonalDataErrors);
        return PartialView("_StatusMessage", "Ошибка при удалении пользователя!");
      }

      _logger.LogInformation("Delete personal data has succeeded. User {id}.", user.Id);

      await _signInManager.SignOutAsync();
      _logger.LogInformation("Delete personal data. User {id}. Sign out.", user.Id);

      return Ok();
    }
  }
}