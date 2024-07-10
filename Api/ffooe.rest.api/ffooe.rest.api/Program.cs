using ffooe.db.context;
using Microsoft.EntityFrameworkCore;
using SimplePatch;
using ffooe.db.entities;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OAuth;
;

DeltaConfig.Init(cfg => {
    cfg.AddEntity<M_Client>();
});

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true).Build();
var connString = configuration.GetConnectionString("ffooe");

builder.Services.AddControllers();
builder.Services.AddDbContext<FFOOEContext>(options => options.UseSqlServer(connString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   c.EnableAnnotations();
   c.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "FFOOE API");
    c.RoutePrefix = string.Empty;
});

//}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
