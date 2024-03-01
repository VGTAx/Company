using Company.Middlewares;
using Company.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Host.ConfigureSerilog();
builder.Services.ConfigureDefaultIdentity();
builder.Services.ConfigureDbContext();
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureControllersWithViews();
builder.Services.ConfigureSmtpSettings();
builder.Services.AddCustomServices();

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
