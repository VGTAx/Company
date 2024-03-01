using Company.IServices;
using Company.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Company.Middlewares
{
  /// <summary>
  /// Middleware Class для изменения ролей пользователя.
  /// </summary>
  public class ChangeRoleMiddleware
  {
    private readonly RequestDelegate _next;
    /// <summary>
    /// Создает экземпляр класса <see cref="ChangeRoleMiddleware"/>.
    /// </summary>
    /// <param name="next">Следующий делегат запроса.</param>
    public ChangeRoleMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    /// <summary>
    /// Middleware для обработки изменения ролей пользователя.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="_userManager">Менеджер пользователей.</param>
    /// <param name="_changeRole">Сервис уведомлений о смене роли.</param>
    /// <param name="_signInManager">Менеджер аутентификации.</param>
    /// <returns>Асинхронную задачу, представляющую выполнение следующего обработчика запроса.</returns>
    public async Task InvokeAsync(
      HttpContext context,
      UserManager<ApplicationUserModel> _userManager,
      INotificationService _changeRole,
      SignInManager<ApplicationUserModel> _signInManager)
    {
      if(context.User.Identity.Name == null)
      {
        await _next.Invoke(context);
      }
      else
      {
        var user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;

        if(user != null && _changeRole.HasNotification(user.Id))
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
  /// <summary>
  /// Статический класс для добавления ChangeRoleMiddleware в конвейер обработки запросов.
  /// </summary
  public static class ChangeRoleMiddlewareExtensions
  {
    /// <summary>
    /// Добавляет <see cref="ChangeRoleMiddleware"/> в конвейер обработки запросов.
    /// </summary>
    /// <param name="builder">Builder ASP.NET.</param>
    /// <returns>Builder ASP.NET с добавленным <see cref="ChangeRoleMiddleware"/>.</returns>
    public static IApplicationBuilder UseChangeRoleMiddleware(
        this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ChangeRoleMiddleware>();
    }
  }
}
