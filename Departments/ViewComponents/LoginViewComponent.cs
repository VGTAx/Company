using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.ViewComponents
{
  [ViewComponent]
  public class LoginViewComponent : ViewComponent
  {
    private readonly UserManager<ApplicationUserModel>? _userManger;
    public LoginViewComponent(UserManager<ApplicationUserModel> userManager)
    {
      _userManger = userManager;
    }

    public IViewComponentResult Invoke()
    {
      var user = _userManger.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User).GetAwaiter().GetResult();

      return View(user);
    }


  }
}
