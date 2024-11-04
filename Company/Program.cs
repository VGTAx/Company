using Company;

var startup = new Startup();
var builder = WebApplication.CreateBuilder(args);
SetConfig();

startup.ConfigureService(builder.Services, builder.Configuration);
builder.WebHost.UseUrls($"http://*:{builder.Configuration["Port"]}");

var app = builder.Build();
startup.ConfigureApp(app);

app.Run();

void SetConfig()
{
  var configPath = AppDomain.CurrentDomain.BaseDirectory + "/Config";
  builder.Configuration.SetBasePath(configPath);
  var jsonFiles = new DirectoryInfo(configPath)
    .GetFiles()
    .Select(c => c.Name)
    .ToList();

  jsonFiles.ForEach(file => builder.Configuration.AddJsonFile(file));
  builder.Configuration.AddEnvironmentVariables("COMPANY:");

  builder.Configuration["ConfigBasePath"] = configPath;
}