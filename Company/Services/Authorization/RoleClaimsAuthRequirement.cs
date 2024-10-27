using Microsoft.AspNetCore.Authorization;

namespace Company.Services.Authorization
{
  public class RoleClaimsAuthRequirement : IAuthorizationRequirement
  {
    public RoleClaimsAuthRequirement() { }
    public RoleClaimsAuthRequirement(params RoleClaims[] roleClaims)
    {
      RoleClaims = RoleClaimsToString(roleClaims);
    }

    public string[]? RoleClaims { get; set; }

    private string[] RoleClaimsToString(params RoleClaims[] roleClaims)
    {
      List<string> roles = new();

      foreach(var role in roleClaims)
      {
        roles.Add(role.ToString());
      }

      return roles.ToArray();
    }
  }
}

public enum RoleClaims
{
  Admin,
  Manager,
  User
}