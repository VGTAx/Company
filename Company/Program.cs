using Company.Data;
using Company.Filters;
using Company.Interfaces;
using Company.IServices;
using Company.Middlewares;
using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration["MySqlConnection"];
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
    policy.RequireRole("Admin");
  });
  options.AddPolicy("ManagePolicy", policy =>
  {
    policy.RequireRole("Admin", "Manager");
  });
  options.AddPolicy("BasicPolicy", policy =>
  {
    policy.RequireRole("Admin", "Manager", "User");
  });

});
builder.Services.AddMemoryCache();
builder.Services.AddMvc();
builder.Services.AddControllersWithViews(options =>
{
  options.Filters.Add<CheckUserExistFilter>();
});
builder.Services.Configure<SmtpSettings>(
  builder.Configuration.GetSection(nameof(SmtpSettings)));
builder.Services.AddScoped<IEmailSender, MailKitEmailSenderService>();
builder.Services.AddScoped<IUserRoleClaims<ApplicationUserModel>, UserRoleClaimsService>();
builder.Services.AddSingleton<INotificationService, ChangeRoleNotificationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
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
