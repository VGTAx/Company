using Microsoft.AspNetCore.Mvc;

namespace Company.Models.ViewModels
{
  /// <summary>
  /// 
  /// </summary>
  public class AccessSettingsPoco
  {
    /// <summary>
    /// 
    /// </summary>
    public ApplicationUserModel? User { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string>? UserRoles { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string>? Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [HiddenInput]
    public List<string>? SelectedRoles { get; set; }
  }
}
