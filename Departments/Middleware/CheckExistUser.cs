using Company.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Company.Middleware
{
    public class CheckExistUser
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CheckExistUser (RequestDelegate next, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _next = next;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userName = context.User.Identity.Name;
            if(userName != null )
            {
                var user = await _userManager.FindByNameAsync(userName);
                if(user != null)
                {
                   await _signInManager.SignOutAsync();
                }
            }
            await _next(context);

        }
    }
}
