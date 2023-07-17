using Company.Data;
using Company.Filters;
using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CompanyContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection")!,
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))));

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
  options.AddPolicy("AdminOnlyPolicy", policy =>
  {
    policy.RequireRole("Admin");
  });
  options.AddPolicy("MyPolicy", policy =>
  {
    policy.RequireRole("User", "Admin");
  });
  
});

builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews(options =>
{
  options.Filters.Add<CheckUserExistFilter>();
});
builder.Services.AddMvc();
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();

builder.Services.AddTransient<AdminAccountService>();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
  var services = serviceScope.ServiceProvider;

  var myDependency = services.GetRequiredService<AdminAccountService>();

  //Use the service
  await myDependency.CreateAdminAccount();

}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Department}/{action=Index}/{id?}");

app.Run();
