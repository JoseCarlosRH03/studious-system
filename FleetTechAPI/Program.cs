using FleetTechAPI;
using FleetTechAPI.Routes;
using FleetTechAPI.Services;
using FleetTechAPI.Services.Data;
using FleetTechAPI.Services.Infrastructure;
using FleetTechCore;
using FleetTechCore.Logic;
using FleetTechCore.Services;
using FleetTechCore.Services.Infrastructure;
using FleetTechCore.Services.Model_Related_Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JsonOptions>(config =>
{
    config.SerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
    config.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IResourceService, ResourceService>();

builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.Configure<EmailService.MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddDbContext<DataService>(options => {
    var env = builder.Environment;
    if (env.IsLocalDevelopment()) {
        options.UseSqlite(builder.Configuration.GetConnectionString("FleetTechLite"));
    } else options.UseMySQL(builder.Configuration.GetConnectionString("FleetTech"));
});
builder.Services.AddScoped<Logic>();
builder.Services.AddScoped<Context>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (HostEnvironmentEnvExtensions.IsDevelopment(app.Environment))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapUserManagement();
app.MapFleetManagement();

app.Run();

namespace FleetTechAPI {
    internal class SnakeCaseNamingPolicy : System.Text.Json.JsonNamingPolicy {
        public static readonly SnakeCaseNamingPolicy Instance = new();
        public override string ConvertName(string name) => name.PascalCaseWithInitialsToStrings().Join("_").ToLower();
    }
    public record LoginParameters(string email, string password);
}