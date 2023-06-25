using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Company.Data;
using Microsoft.AspNetCore.Authentication;

namespace Company.Filters
{
    public class CheckUserExistFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckUserExistFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //public async Task OnActionExecutedAsync(ActionExecutedContext context)
        //{
        //    var userName = context.HttpContext.User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(userName) != null;
        //    if (!user)
        //    {
        //        await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //    }
        //}

        //public async Task OnActionExecutingAsync(ActionExecutingContext context)
        //{
        //    var userName = context.HttpContext.User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(userName) != null;
        //    if(!user)
        //    {
        //       await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //    }
        //}

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userName = context.HttpContext.User.Identity.Name;
            if(userName is not null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                }
            }

            
           

            
        }
    }
}
