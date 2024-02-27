using Company.Interfaces;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.BaseClass
{
  public abstract class AccountServiceBase : ICheckEmailService, IErrorMessage, IGenerateTokenService<ApplicationUserModel>
  {
    public abstract Task<bool> CheckIsEmailExistAsync(string email);
    public abstract Task<string> GenereateEmailConfirmationTokenAsync(ApplicationUserModel user);
    public abstract Task<string> GenereatePasswordResetTokenAsync(ApplicationUserModel user);
    public abstract IEnumerable<string> GetIdentityResultErrors(IdentityResult errors);
    public abstract IEnumerable<string> GetModelStateErrors(ModelStateDictionary errors);
  }
}

