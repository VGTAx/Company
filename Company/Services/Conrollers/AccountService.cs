using Company.BaseClass;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Company.Services.Conrollers
{
  public class AccountService : AccountServiceBase
  {
    private readonly UserManager<ApplicationUserModel> _userManager;

    public AccountService(UserManager<ApplicationUserModel> userManager)
    {
      _userManager = userManager;
    }

    public override async Task<bool> CheckIsEmailExistAsync(string email)
    {
      return await _userManager.FindByEmailAsync(email) is null;
    }

    public override async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserModel user)
    {
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
      return token;
    }

    public override async Task<string> GeneratePasswordResetTokenAsync(ApplicationUserModel user)
    {
      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
      return token;
    }
  }
}
