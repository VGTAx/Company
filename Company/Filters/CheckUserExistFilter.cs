using Company.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Filters
{
  /// <summary>
  /// Фильтр проверяет существование пользователя.
  /// </summary>
  /// <remarks>
  /// Данный фильтр использует UserManager для проверки существования пользователя и MemoryCache для кэширования результатов.
  /// </remarks>
  public class CheckUserExistFilter : IAsyncAuthorizationFilter
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    /// <summary>
    /// Создает экземпляр класса <see cref="CheckUserExistFilter"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей для управления пользователями.</param>
    /// <param name="cache">Кэш в памяти для временного хранения данных.</param>
    public CheckUserExistFilter(UserManager<ApplicationUserModel> userManager)
    {
      _userManager = userManager;
    }
    /// <summary>
    /// Выполняет асинхронную проверку авторизации на основе контекста фильтра.
    /// </summary>
    /// <param name="context">Контекст фильтра авторизации.</param>
    /// <returns>Объект Task представляющий асинхронную операцию проверки авторизации.</returns>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      var userName = context.HttpContext.User.Identity!.Name;

      if(!string.IsNullOrEmpty(userName))
      {
        // Проверяем, было ли уже выполнено перенаправление
        var user = await _userManager!.FindByNameAsync(userName);
        if(user == null)
        {
          await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          context.Result = new RedirectToActionResult("Logout", "Account", null);
        }
      }
    }
  }
}
