using ffooe.db.context;
using ffooe.service.worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
                        //.AddJsonFile("appsettings.json", true, true)
#if DEBUG
                        .AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ffooe-secrets.json"), false, true).Build();
#else
                        .AddJsonFile("C:\\inetpub\\secrets\\ffooe-api-secrets.json", true, true).Build();
#endif
var connString = configuration.GetConnectionString("ffooe");
builder.Configuration.AddConfiguration(configuration);
builder.Services.AddHostedService<PushGroupCreateWorker>();
builder.Services.AddDbContext<FFOOEContext>(options => options.UseSqlServer(connString));

var host = builder.Build();
host.Run();
