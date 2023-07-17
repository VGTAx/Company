using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.ViewComponents
{
  [ViewComponent]
  public class AdminViewComponent : ViewComponent
  {
    private readonly UserManager<ApplicationUserModel>? _userManger;

    public AdminViewComponent(UserManager<ApplicationUserModel>? userManger)
    {
      _userManger = userManger;
    }

    public IViewComponentResult Invoke()
    {
      var user = _userManger.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User).GetAwaiter().GetResult();

      return View(user);
    }
  }
}
