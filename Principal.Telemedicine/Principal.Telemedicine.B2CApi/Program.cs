using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Identity.Web;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging;
using Principal.Telemedicine.SharedApi.Models;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
var hostBuilder = new HostBuilder()
           .ConfigureServices((context, services) =>
           {
               //... registrace DbContextGeneral
               services.AddDbContext<DbContextGeneral> (options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("TMWorkstore")));
               // Register the DiRegistration class
               services.AddLogging(configuration);
           });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDownstreamWebApi("ExtendedPropertiesController", builder.Configuration.GetSection("ExtendedPropertiesControllerScope"))
    .AddInMemoryTokenCaches();

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

//Dependency Injection od DbContext Class
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VANDA_TEST")));

builder.Services.AddLogging(configuration);

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

