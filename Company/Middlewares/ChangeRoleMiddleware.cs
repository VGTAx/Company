using Company_.Models;
using Company_.IServices;
using Company_.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Company_.Middlewares
{
  public class ChangeRoleMiddleware
  {
    private readonly RequestDelegate _next;

    public ChangeRoleMiddleware(RequestDelegate next)
    {      
      _next = next;      
    }

    public async Task InvokeAsync(HttpContext context,  UserManager<ApplicationUserModel> _userManager, INotificationService _changeRole, SignInManager<ApplicationUserModel> _signInManager)//метод, который обрабатывает запрос
    {
      if (context.User.Identity.Name == null)
      {
        await _next.Invoke(context);
      }
      else
      {
        var user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;        

        if (_changeRole.HasNotification(user.Id))
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
