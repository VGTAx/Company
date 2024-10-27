using Company;


var startup = new Startup();

var builder = WebApplication.CreateBuilder(args);
startup.ConfigureService(builder.Services, builder.Configuration);

var app = builder.Build();
startup.ConfigureApp(app);

app.Run();
