using Company.BaseClass;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Company.Services
{
  public class ManageAccount : ManageAccountBase<AppUser>
  {
    private readonly UserManager<AppUser> _userManager;

    public ManageAccount(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public override async Task<string> GenerateChangeEmailTokenAsync(AppUser user, string email)
    {
      var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

      return token;
    }
  }
}
