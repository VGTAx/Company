using Company.Interfaces;
using Company.Models;

namespace Company.BaseClass
{
  public abstract class AccountBase : ErrorMessageBase, ICheckEmailService
  {
    public abstract Task<bool> CheckIsEmailExistAsync(string email);
    public abstract Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
    public abstract Task<string> GeneratePasswordResetTokenAsync(AppUser user);
  }
}

