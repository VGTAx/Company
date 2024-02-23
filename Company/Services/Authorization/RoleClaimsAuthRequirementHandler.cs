using Microsoft.AspNetCore.Authorization;

namespace Company.Services.Authorization
{
  public class RoleClaimsAuthRequirementHandler
    : AuthorizationHandler<RoleClaimsAuthRequirement>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimsAuthRequirement requirement)
    {
      var userClaims = context.User.Claims.ToList();
      var roleClaims = requirement.RoleClaims;
      bool containsAnyRoleClaims =
        roleClaims?.Any(claim => userClaims?.Any(userClaim => userClaim.Value == claim) ?? false) ?? false;

      if(containsAnyRoleClaims)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
