using Company.Models;
using Company.Models.ManageAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    public class ManageAccountController : Controller
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;

        public ManageAccountController(
            UserManager<ApplicationUserModel> userManager,
            SignInManager<ApplicationUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult _ManageNav()
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

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile([Bind("Email,Name, Phone")] ProfileModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Index", "Department");
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if(!ModelState.IsValid)
            {
                model.StatusMessage = "Ошибка при обновлении профиля";
                return PartialView(model);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (model.Phone != phoneNumber)
            {
                var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
                if (!setPhoneNumberResult.Succeeded)
                {
                    model.StatusMessage = "Ошибка при обновлении профиля";
                    return PartialView(model);
                }
            }
            if(user.Name != model.Name)
            {
                user.Name = model.Name;
                var upadateNameResult = await _userManager.UpdateAsync(user);                
                if(!upadateNameResult.Succeeded)
                {
                    model.StatusMessage = "Ошибка при обновлении профиля";
                    return PartialView(model);
                }
            }
            await _signInManager.RefreshSignInAsync(user);            
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEmail()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email =  await _userManager.GetEmailAsync(user);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            var model = new EmailModel
            {
                Email = email,
                IsEmailConfirmed = isEmailConfirmed,
            };                      

            return PartialView(model);
        }
    }
}
