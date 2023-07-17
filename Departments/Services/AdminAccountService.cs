using Company.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace Company.Services
{
  public class AdminAccountService
  {
    private readonly UserManager<ApplicationUserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
 

    public AdminAccountService(UserManager<ApplicationUserModel> userManager, 
      RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      
    }

    public async Task CreateAdminAccount()
    {
      var adminUser = await _userManager.FindByEmailAsync("putinvodkagta@yandex.by");
      if(adminUser == null)
      {
        var newAdminUser = new ApplicationUserModel
        {
          UserName = "putinvodkagta@yandex.by",          
          Email = "putinvodkagta@yandex.by",          
          EmailConfirmed = true,          
          Name = "Admin"
        };

        

        var result = await _userManager.CreateAsync(newAdminUser, "Password123!");
        if (result.Succeeded)
        {
          var role = await _roleManager.FindByNameAsync("Admin");
          var roleClaim = new Claim(ClaimTypes.Role, role.Name);

          await _userManager.AddClaimAsync(newAdminUser, roleClaim);
        }
        else
        {
          throw new ArgumentNullException();
        }
        
      }
    }
  }
}
