using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Principal.Telemedicine.B2CApi;
using Principal.Telemedicine.B2CApi.Controllers;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging;
using Microsoft.Extensions.Hosting;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.SharedApi.Models;
using Moq;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
var secretFilePath = "Secured/secrets.json";


//var host = hostBuilder.Build();
//var options = host.Services.GetService(IOptions<AuthorizationSettings>);

builder.Services.AddDbContext<DbContextGeneral>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TMWorkstore")));

//Dependency Injection od DbContext Class
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VANDA_TEST")));

builder.Services.AddLogging(configuration);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LoggingMiddleware>();

app.Run();

