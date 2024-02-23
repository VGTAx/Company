using Company.Data;
using Company.Filters;
using Company.Interfaces;
using Company.IServices;
using Company.Middlewares;
using Company.Models;
using Company.Services;
using Company.Services.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
builder.Services.AddDbContext<ICompanyContext, CompanyContext>(
  options => options.UseMySql(connectionString!, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDefaultIdentity<ApplicationUserModel>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
   {
     options.LoginPath = "/Account/Login";
     options.LogoutPath = "/Account/Logout";
     options.AccessDeniedPath = "/Account/Login";
   });
builder.Services.AddAuthorization(options =>
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
builder.Services.AddMemoryCache();
builder.Services.AddMvc();
builder.Services.AddControllersWithViews(options =>
{
  options.Filters.Add<CheckUserExistFilter>();
});
builder.Services.Configure<SmtpSettings>(config =>
{
  config.Host = Environment.GetEnvironmentVariable("SMTP_HOST");
  config.Port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port) ? port : throw new ArgumentNullException(nameof(config.Port), "SMTP:PORT is null");
  config.Email = Environment.GetEnvironmentVariable("SMTP_EMAIL");
  config.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
  config.SenderName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME");
}

);
builder.Services.AddScoped<IEmailSender, MailKitEmailSenderService>();
builder.Services.AddScoped<IUserRoleClaims<ApplicationUserModel>, UserRoleClaimsService>();
builder.Services.AddSingleton<INotificationService, ChangeRoleNotificationService>();
builder.Services.AddSingleton<IAuthorizationHandler, RoleClaimsRequirementHandle>();

var app = builder.Build();

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


app.Run();
