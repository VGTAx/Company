using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.Interfaces
{
  public interface IErrorMessage
  {
    IEnumerable<string> GetModelStateErrors(ModelStateDictionary errors);
    IEnumerable<string> GetIdentityResultErrors(IdentityResult errors);
  }
}
