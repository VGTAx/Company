using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.Interfaces
{
  public interface IErrorMessage
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    IEnumerable<string> GetModelErrors(ModelStateDictionary errors);
    /// <summary>
    ///
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    IEnumerable<string> GetIdentityResultErrors(IdentityResult errors);
  }
}
