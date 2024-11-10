using Company.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.BaseClass
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class ErrorMessageBase : IErrorMessage
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public IEnumerable<string> GetIdentityResultErrors(IdentityResult errors)
    {
      return errors.Errors.Select(c => c.Description);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public IEnumerable<string> GetModelErrors(ModelStateDictionary errors)
    {
      return errors.Values.SelectMany(v => v.Errors)
                          .Select(e => e.ErrorMessage);
    }
  }
}
