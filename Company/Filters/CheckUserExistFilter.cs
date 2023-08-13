using Company_.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Company_.Filters
{
  public class CheckUserExistFilter : IAsyncAuthorizationFilter
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly IMemoryCache _cache;
    private readonly SignInManager<ApplicationUserModel> _signInManager;

    public CheckUserExistFilter(UserManager<ApplicationUserModel> userManager, IMemoryCache cache, SignInManager<ApplicationUserModel> signInManager)
    {
      _userManager = userManager;
      _cache = cache;
      _signInManager = signInManager; 
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      var userName = context.HttpContext.User.Identity.Name;

      if (!string.IsNullOrEmpty(userName))
      {
        // Проверяем, было ли уже выполнено перенаправление
        var alreadyRedirected = _cache.TryGetValue($"{userName}_AlreadyRedirected", out bool cachedAlreadyRedirected) && cachedAlreadyRedirected;
        ApplicationUserModel user = await _userManager.FindByNameAsync(userName)!;
        if (!alreadyRedirected && user == null)
        {
          if (!_cache.TryGetValue($"{userName}_SecurityStamp", out string cachedSecurityStamp))
          {
            // Если значение отсутствует в кэше, получение его из базы данных                    
            cachedSecurityStamp = user?.SecurityStamp ?? string.Empty;

            // Сохранение значения SecurityStamp в кэше
            _cache.Set($"{userName}_SecurityStamp", cachedSecurityStamp);
          }

          //var userSecurityStamp = user != null ? await _userManager.GetSecurityStampAsync(user) : null;

          //if (cachedSecurityStamp != userSecurityStamp)
          //{
          //  // Устанавливаем флаг в MemoryCache, чтобы пометить, что уже выполнено перенаправление или выход из аккаунта
          //  //_cache.Set($"{userName}_AlreadyRedirected", true);

          //  var userClaimPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
          //  //await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          //  //await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userClaimPrincipal);
          //  await _signInManager.RefreshSignInAsync(user);
          //  //context.Result = new RedirectToActionResult("Logout", "Account", null);
          //}
          // Устанавливаем флаг в MemoryCache, чтобы пометить, что уже выполнено перенаправление или выход из аккаунта
          _cache.Set($"{userName}_AlreadyRedirected", true);

          await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          context.Result = new RedirectToActionResult("Logout", "Account", null);
        }
      }
    }
  }
}
