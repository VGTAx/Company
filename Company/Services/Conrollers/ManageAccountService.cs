using Company.BaseClass;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Company.Services
{
  public class ManageAccountService : ManageAccountBase<ApplicationUserModel>
  {
    private readonly UserManager<ApplicationUserModel> _userManager;

    public ManageAccountService(UserManager<ApplicationUserModel> userManager)
    {
      _userManager = userManager;
    }

    public override async Task<string> GenerateChangeEmailTokenAsync(ApplicationUserModel user, string email)
    {
      var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

      return token;
    }
  }
}
