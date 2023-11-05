using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Identity.Web;
using Principal.Telemedicine.B2CApi;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.Shared.Logging;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
var secretFilePath = "Secured/secrets.json";

builder.Services.AddDbContext<DbContextApi>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VANDA_TEST")));

builder.Services.AddLogging(configuration);
builder.Services.AddScoped<IADB2CRepository, ADB2CRepository>();
builder.Services.AddSecretConfiguration<AuthorizationSettings>(configuration, secretFilePath);
builder.Services.AddSecretConfiguration<Principal.Telemedicine.Shared.Configuration.AzureAdB2C>(configuration, secretFilePath);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = (builder.Configuration.GetConnectionString("ApplicationInsights")),
            configureApplicationInsightsLoggerOptions: (options) => { }
    );

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("B2CApi", LogLevel.Trace);

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();


if (app.Environment.IsLocalHosted())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LoggingMiddleware>();

app.Run();

