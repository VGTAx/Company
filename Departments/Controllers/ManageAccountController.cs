using Company.Data;
using Company.Models.ManageAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    public class ManageAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageAccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            };

            var userName = await _userManager.GetUserNameAsync(user);
            var userPhone = await _userManager.GetPhoneNumberAsync(user);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile([Bind("Email, Phone")] ProfileModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if(!ModelState.IsValid)
            {
                return View();
            }
           
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (model.Phone != phoneNumber)
            {
                var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.Phone);
                if (!setPhoneNumberResult.Succeeded)
                {
                    ModelState.AddModelError("", "Ошибка при добавлении номера телефона");
                    return View();
                }
            }
            await _signInManager.RefreshSignInAsync(user);            

            return View();
        }
    }
}
