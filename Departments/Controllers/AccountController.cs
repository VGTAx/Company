using Company.Areas.Identity.Pages.Account;
using Company.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Company.Models;
using System.Text.Encodings.Web;
using System.Security.Claims;

namespace Company.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<Areas.Identity.Pages.Account.RegisterModel> _logger;
        private readonly IEmailSender _emailSender; 
       
        private string? ReturnUrl { get; set; }

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<Areas.Identity.Pages.Account.RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore()!;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender; 
        }
       
        public async Task<IActionResult> ConfirmEmail(string userId, string code) 
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index","Department");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmEmail", "Account");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Email, Name, Password, ConfirmPassword")] Models.RegisterModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var claim = new Claim("Name", model.Name, ClaimValueTypes.String);
                    await _userManager.AddClaimAsync(user, claim);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callBackUrl = Url.Action(
                        action: "ConfirmEmail",
                        controller: "Account",
                        values: new { userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _userManager.AddToRoleAsync(user, "user");                    
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Для подтверждения регистрации <a href='{HtmlEncoder.Default.Encode(callBackUrl!)}'>нажмите сюда</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("ConfirmEmail", "Account");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }               
            }     
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email, Password, RememberMe")] Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Department");
        }


        private IUserEmailStore<ApplicationUser>? GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
