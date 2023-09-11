﻿using Company.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Company.Filters
{
  public class CheckUserExistFilter : IAsyncAuthorizationFilter
  {
    private readonly UserManager<ApplicationUserModel>? _userManager;
    private readonly IMemoryCache? _cache;

    public CheckUserExistFilter(UserManager<ApplicationUserModel> userManager, IMemoryCache cache)
    {
      _userManager = userManager;
      _cache = cache;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      var userName = context.HttpContext.User.Identity!.Name;

      if (!string.IsNullOrEmpty(userName))
      {
        // Проверяем, было ли уже выполнено перенаправление
        var alreadyRedirected = _cache.TryGetValue($"{userName}_AlreadyRedirected", out bool cachedAlreadyRedirected) && cachedAlreadyRedirected;
        var user = await _userManager.FindByNameAsync(userName);
        if (!alreadyRedirected && user == null)
        {
          if (!_cache.TryGetValue($"{userName}_SecurityStamp", out string? cachedSecurityStamp))
          {
            // Если значение отсутствует в кэше, получение его из базы данных                    
            cachedSecurityStamp = user?.SecurityStamp ?? string.Empty;

            // Сохранение значения SecurityStamp в кэше
            _cache.Set($"{userName}_SecurityStamp", cachedSecurityStamp);
          }
          _cache.Set($"{userName}_AlreadyRedirected", true);

          await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          context.Result = new RedirectToActionResult("Logout", "Account", null);
        }
      }
    }
  }
}
