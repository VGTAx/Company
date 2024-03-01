using Company.BaseClass;
using Company.Data;
using Company.Filters;
using Company.Interfaces;
using Company.IServices;
using Company.Models;
using Company.Models.Department;
using Company.Models.Employee;
using Company.Services.Authorization;
using Company.Services.Conrollers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Company.Services
{
  public static class ServiceRegistrationExtensions
  {
    public static void AddCustomServices(this IServiceCollection services)
    {
      services.AddScoped<IEmailSender, MailKitEmailSenderService>();
      services.AddScoped<IUserRoleClaims<ApplicationUserModel>, UserRoleClaimsService>();
      services.AddScoped<AccountServiceBase, AccountService>();
      services.AddScoped<ManageAccountBase<ApplicationUserModel>, ManageAccountService>();
      services.AddScoped<EmployeeServiceBase<EmployeeModel>, EmployeeService>();
      services.AddScoped<DepartmentServiceBase<DepartmentModel>, DepartmentService>();
      services.AddSingleton<INotificationService, ChangeRoleNotificationService>();
      services.AddSingleton<IAuthorizationHandler, RoleClaimsAuthRequirementHandler>();
    }

    public static void ConfigureSmtpSettings(this IServiceCollection services)
    {
      services.Configure<SmtpSettings>(config =>
       {
         config.Host = Environment.GetEnvironmentVariable("SMTP_HOST");
         config.Port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port) ? port : throw new ArgumentNullException(nameof(config.Port), "SMTP:PORT is null");
         config.Email = Environment.GetEnvironmentVariable("SMTP_EMAIL");
         config.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
         config.SenderName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME");
       });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
      services.AddAuthorization(options =>
       {
         options.AddPolicy("AdminPolicy", policy =>
         {
           policy.AddRequirements(new RoleClaimsAuthRequirement("Admin"));
         });
         options.AddPolicy("ManagePolicy", policy =>
         {
           policy.AddRequirements(new RoleClaimsAuthRequirement("Admin", "Manager"));
         });
         options.AddPolicy("BasicPolicy", policy =>
         {
           policy.AddRequirements(new RoleClaimsAuthRequirement("Admin", "Manager", "User"));
         });

       });
    }

    public static void ConfigureAuthentication(this IServiceCollection services)
    {
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.LoginPath = "/Account/Login";
          options.LogoutPath = "/Account/Logout";
          options.AccessDeniedPath = "/Account/Login";
        });
    }

    public static void ConfigureControllersWithViews(this IServiceCollection services)
    {
      services.AddControllersWithViews(options =>
       {
         options.Filters.Add<CheckUserExistFilter>();
       });
    }

    public static void ConfigureDbContext(this IServiceCollection services)
    {
      var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

      services.AddDbContext<ICompanyContext, CompanyContext>(
        options => options.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString)));
    }

    public static void ConfigureDefaultIdentity(this IServiceCollection services)
    {
      services.AddDefaultIdentity<ApplicationUserModel>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();
    }

    public static void ConfigureSerilog(this ConfigureHostBuilder hostBuilder)
    {
      hostBuilder.UseSerilog((context, configuration) =>
      {
        configuration.ReadFrom.Configuration(context.Configuration);
      });
    }
  }
}
