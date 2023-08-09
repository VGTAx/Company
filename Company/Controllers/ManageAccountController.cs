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
  
  [Authorize(Policy = "MyPolicy", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class ManageAccountController : Controller
  {
    private readonly UserManager<ApplicationUserModel> _userManager;
    private readonly SignInManager<ApplicationUserModel> _signInManager;
    private readonly IEmailSender _emailSender;

    public ManageAccountController(
        UserManager<ApplicationUserModel> userManager,
        SignInManager<ApplicationUserModel> signInManager,
        IEmailSender emailSender)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
    }  

    public IActionResult _ManageAccountNav()
    {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Ошибка, невозможно получить данные. Авторизуйтесь повторно");
      }

      var model = new ProfileModel
      {
        Email = user.Email,
        Name = user.Name,
        Phone = user.PhoneNumber,
      };
      ViewData["ActiveLink"] = "profile";
      return PartialView(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile([Bind("Email,Name, Phone")] ProfileModel model)
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return RedirectToAction("Index", "Department");
      }
      if (!ModelState.IsValid)
      {
        ViewData["StatusMessage"] = "Ошибка при обновлении профиля";
        return PartialView(model);
      }

      var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

      if (model.Phone != phoneNumber)
      {
        var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
        if (!setPhoneNumberResult.Succeeded)
        {
          ViewData["StatusMessage"] = "Ошибка при обновлении профиля";
          return PartialView(model);
        }
        ViewData["StatusMessage"] = "Профиль изменен"!;
      }
      if (user.Name != model.Name)
      {
        user.Name = model.Name;
        var upadateNameResult = await _userManager.UpdateAsync(user);
        if (!upadateNameResult.Succeeded)
        {
          ViewData["StatusMessage"] = "Ошибка при обновлении профиля";
          return PartialView(model);
        }
        ViewData["StatusMessage"] = "Профиль изменен"!;
      }
      if (ViewData["StatusMessage"]!.ToString() == "Профиль изменен"!)
      {
        await _signInManager.RefreshSignInAsync(user);
        return PartialView("_StatusMessage", ViewData["StatusMessage"]);
      }
      else
      {
        return PartialView(model);
      }
    }

    [HttpGet]
    public async Task<IActionResult> ChangeEmail()
    {
      var user = await _userManager.GetUserAsync(User);

      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var email = await _userManager.GetEmailAsync(user);
      var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

      var model = new ChangeEmailModel
      {
        Email = email,
        IsEmailConfirmed = isEmailConfirmed,
      };

      return PartialView(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmail([Bind("NewEmail, IsEmailConfirmed")] ChangeEmailModel model)
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        ViewData["StatusMessage"] = "Ошибка, пользователь не найден!";
        return PartialView("_StatusMessage");
      }

      model.Email = await _userManager.GetEmailAsync(user);

      if (!ModelState.IsValid || String.IsNullOrEmpty(model.NewEmail))
      {
        ModelState.AddModelError("NewEmail", "Введите новую электронную почту");
        return PartialView(model);
      }

      var userId = await _userManager.GetUserIdAsync(user);
      var checkAvailableEmail = await _userManager.FindByEmailAsync(model.NewEmail);

      if (model.NewEmail != model.Email && checkAvailableEmail is null)
      {
        var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail!);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Action(
            action: "ChangeEmailConfirmation",
            controller: "ManageAccount",
            values: new { userId = userId, email = model.NewEmail, code = code },
            protocol: Request.Scheme);

        var message = $"Добрый день. Подтвердите изменение эл.почты <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите сюда</a>";
        await _emailSender.SendEmailAsync(model.NewEmail!, "Подтверждение изменения электронной почты", message);

        ViewData["StatusMessage"] = "Пожалуйста, проверьте электронную почту, чтобы подтвердить изменения";
        return PartialView("_StatusMessage");
      }
      ModelState.AddModelError("NewEmail", "Электронная почта уже используется");
      return PartialView(model);
    }

    public async Task<IActionResult> ChangeEmailConfirmation(string userId, string email, string code)
    {
      if (userId == null || code == null || email == null)
      {
        ViewData["StatusMessage"] = $"Пожалуйста, проверьте электронную почту, чтобы подтвердить свою учетную запись.";
        return PartialView("_StatusMessage");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return PartialView("_StatusMessage", "Ошибка! Пользователь не найден");
      }

      code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
      var resultChangeEmail = await _userManager.ChangeEmailAsync(user, email, code);
      if (!resultChangeEmail.Succeeded)
      {
        ViewData["StatusMessage"] = "Ошибка при подтверждении электронной почты";
        return PartialView("_StatusMessage");
      }
      await _userManager.SetUserNameAsync(user, email);

      ViewData["StatusMessage"] = "Электронная почта изменена";
      await _signInManager.RefreshSignInAsync(user);
      return View("_StatusMessage");
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([Bind("OldPassword, NewPassword, ConfirmPassword")] ChangePasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return PartialView();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        ViewData["StatusMessage"] = $"Ошибка! Пользователь не найден.";
        return PartialView("_StatusMessage");
      }

      var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
      if (!changePasswordResult.Succeeded)
      {
        foreach (var error in changePasswordResult.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return PartialView();
      }
      await _signInManager.RefreshSignInAsync(user);

      ViewData["StatusMessage"] = "Пароль был изменен!";
      return PartialView("_StatusMessage");
    }

    [HttpGet]
    public IActionResult PersonalData()
    {
      return PartialView();
    }
    [HttpGet]
    public async Task<IActionResult> DeletePersonalData()
    {
      var user = await _userManager.GetUserAsync(User);
      var model = new DeletePersonalDataModel
      {
        RequirePassword = await _userManager.HasPasswordAsync(user),
        Password = "",
      };

      return PartialView(model);
    }
    [HttpPost]
    public async Task<IActionResult> DeletePersonalData([Bind("RequirePassword, Password")] DeletePersonalDataModel model)
    {
      var user = await _userManager.GetUserAsync(User);

      model.RequirePassword = await _userManager.HasPasswordAsync(user!);
      if (String.IsNullOrEmpty(model.Password))
      {
        ModelState.AddModelError("Password", "Введите пароль");
        return BadRequest(ModelState);
      }
      if (model.RequirePassword && !await _userManager.CheckPasswordAsync(user!, model.Password))
      {
        ModelState.AddModelError("Password", "Неверный пароль");
        return BadRequest(ModelState);
      }

      var result = await _userManager.DeleteAsync(user!);
      if (!result.Succeeded)
      {
        throw new InvalidOperationException("Неизвестная ошибка при удалении пользователя");
      }

      await _signInManager.SignOutAsync();

      return Ok();
    }
  }
}
