using Company.Interfaces;
using Company.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Company.Services
{
  public class UserRoleClaims : IUserRoleClaims<AppUser>
  {
    private readonly UserManager<AppUser> _userManager;

    public UserRoleClaims(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task ChangeUserRoleClaimsAsync(AppUser user, List<string> userRoles, List<string> newUserRoles)
    {
      var rolesToAdd = newUserRoles.Except(userRoles);
      var rolesToRemove = userRoles.Except(newUserRoles!);

      foreach(var role in rolesToAdd)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager!.AddClaimAsync(user, claimRole);
      }

      foreach(var role in rolesToRemove)
      {
        var claimRole = new Claim(ClaimTypes.Role, role);
        await _userManager!.RemoveClaimAsync(user, claimRole);
      }
    }

    public async Task<List<string>> GetUserRoleClaimsAsync(AppUser user)
    {
      var userClaims = await _userManager!.GetClaimsAsync(user);
      var userRoles = userClaims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

      return userRoles;
    }
  }
}
