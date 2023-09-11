using Company.IServices;
using Company.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Company.Middlewares
{
  public class ChangeRoleMiddleware
  {
    private readonly RequestDelegate _next;

    public ChangeRoleMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    //метод, который обрабатывает запрос
    public async Task InvokeAsync(HttpContext context,
      UserManager<ApplicationUserModel> _userManager,
      INotificationService _changeRole,
      SignInManager<ApplicationUserModel> _signInManager)
    {
      if (context.User.Identity.Name == null)
      {
        await _next.Invoke(context);
      }
      else
      {
        var user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;

        if (user != null && _changeRole.HasNotification(user.Id))
        {
          await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
          await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
          _changeRole.RemoveNotification(user.Id);
        }
        await _next.Invoke(context);
      }
    }
  }

  public static class ChangeRoleMiddlewareExtensions
  {
    public static IApplicationBuilder UseChangeRoleMiddleware(
        this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ChangeRoleMiddleware>();
    }
  }
}
