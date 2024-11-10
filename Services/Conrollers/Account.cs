using Company.BaseClass;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Company.Services.Conrollers
{
  public class Account : AccountBase
  {
    private readonly UserManager<AppUser> _userManager;

    public Account(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public override async Task<bool> CheckIsEmailExistAsync(string email)
    {
      return await _userManager.FindByEmailAsync(email) is null;
    }

    public override async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
    {
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
      return token;
    }

    public override async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
    {
      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
      return token;
    }
  }
}
