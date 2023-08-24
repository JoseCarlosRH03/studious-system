using BackendTemplateAPI;
using BackendTemplateAPI.Services;
using BackendTemplateAPI.Services.Data;
using BackendTemplateAPI.Services.Infrastructure;
using BackendTemplateCore;
using BackendTemplateCore.Services;
using BackendTemplateCore.Services.Infrastructure;
using BackendTemplateCore.Services.Model_Related_Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddScoped<IBillerService, BillerService>();
builder.Services.Configure<EmailService.MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IMunicipiaService, MunicipiaService>();
builder.Services.AddScoped<ISicflexService, SicflexService>();
builder.Services.AddScoped<IReadoutService, ReadoutService>();

builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddDbContext<DataService>(options => {
    var env = builder.Environment;
    if (env.IsLocalDevelopment()) {
        options.UseInMemoryDatabase("billfast");
    } else options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (HostEnvironmentEnvExtensions.IsDevelopment(app.Environment))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace BackendTemplateAPI {
    internal class SnakeCaseNamingPolicy : System.Text.Json.JsonNamingPolicy {
        public static readonly SnakeCaseNamingPolicy Instance = new();
        public override string ConvertName(string name) => name.PascalCaseWithInitialsToStrings().Join("_").ToLower();
    }
    public record LoginParameters(string email, string password);
}