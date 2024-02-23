using Microsoft.AspNetCore.Authorization;

namespace Company.Services.Authorization
{
  public class RoleClaimsAuthRequirement : IAuthorizationRequirement
  {
    public RoleClaimsAuthRequirement() { }
    public RoleClaimsAuthRequirement(params string[] roleClaims)
    {
      RoleClaims = roleClaims;
    }

    public string[]? RoleClaims { get; set; }
  }
}
