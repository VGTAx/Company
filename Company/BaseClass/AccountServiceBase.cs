using Company.Interfaces;
using Company.Models;

namespace Company.BaseClass
{
  public abstract class AccountServiceBase : ErrorMessageBase, ICheckEmailService
  {
    public abstract Task<bool> CheckIsEmailExistAsync(string email);
    public abstract Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserModel user);
    public abstract Task<string> GeneratePasswordResetTokenAsync(ApplicationUserModel user);
  }
}

