using Company.BaseClass;
using Company.Data;
using Company.Filters;
using Company.Interfaces;
using Company.IServices;
using Company.Middlewares;
using Company.Models;
using Company.Models.Departments;
using Company.Models.Employee;
using Company.Services;
using Company.Services.Authorization;
using Company.Services.Conrollers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Company;

public class Startup
{
  public void ConfigureService(IServiceCollection services, IConfiguration configuration)
  {
    // ------------------------------------ Database

    var connectionString = configuration["DB_CON_STR"];

    services.AddDbContext<ICompanyContext, CompanyContext>(
      options => options.UseNpgsql(connectionString!));

    // ------------------------------------ MVC

    services.AddControllersWithViews(options =>
    {
      options.Filters.Add<CheckUserExistFilter>();
    });

    // ------------------------------------ Authetication

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.LoginPath = "/Account/Login";
          options.LogoutPath = "/Account/Logout";
          options.AccessDeniedPath = "/Account/Login";
        });

    // ------------------------------------ Authorization

    services.AddAuthorization(options =>
    {
      options.AddPolicy("AdminPolicy", policy =>
      {
        policy.AddRequirements(new RoleClaimsAuthRequirement(RoleClaims.Admin));
      });
      options.AddPolicy("ManagePolicy", policy =>
      {
        policy.AddRequirements(new RoleClaimsAuthRequirement(RoleClaims.Admin, RoleClaims.Manager));
      });
      options.AddPolicy("BasicPolicy", policy =>
      {
        policy.AddRequirements(new RoleClaimsAuthRequirement(RoleClaims.Admin, RoleClaims.Manager, RoleClaims.User));
      });
    });

    // ------------------------------------ Identity

    services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();

    // ------------------------------------ Logger

    Serilog.Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    var loggerFactory = LoggerFactory.Create(builder =>
    {
      builder.SetMinimumLevel(LogLevel.Trace);
      builder.AddSerilog(Log.Logger);
    });

    services.AddSingleton(loggerFactory);

    // ------------------------------------ SMTP

    services.Configure<SmtpSettings>(config =>
    {
      config.Host = configuration["SMTP_HOST"];
      config.Port = int.TryParse(configuration["SMTP_PORT"], out int port) ? port : throw new ArgumentNullException(nameof(config.Port), "SMTP:PORT is null");
      config.Email = configuration["SMTP_EMAIL"];
      config.Password = configuration["SMTP_PASSWORD"];
      config.SenderName = configuration["SMTP_SENDER_NAME"];
    });

    // ------------------------------------ Other services

    services.AddScoped<IEmailSender, MailKitEmailSender>();
    services.AddScoped<IUserRoleClaims<AppUser>, UserRoleClaims>();
    services.AddScoped<AccountBase, Account>();
    services.AddScoped<ManageAccountBase<AppUser>, ManageAccount>();
    services.AddScoped<EmployeeBase<EmployeeModel>, Employee>();
    services.AddScoped<DepartmentBase<DepartmentModel>, DepartmentService>();

    services.AddSingleton<INotification, ChangeRoleNotification>();
    services.AddSingleton<IAuthorizationHandler, RoleClaimsAuthRequirementHandler>();
    services.AddSingleton<StringParser>();
  }

  public void ConfigureApp(WebApplication app)
  {
    // Configure the HTTP request pipeline.
    if(!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapRazorPages();
    app.UseChangeRoleMiddleware();
    app.MapControllerRoute(
      name: "confirmationRegister",
      pattern: "Account/RegisterConfirmation/{userId}/{code}");
    app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Department}/{action=Index}/{id?}");
  }
}
