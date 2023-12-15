﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Principal.Telemedicine.B2CApi;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.Shared.Logging;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
builder.Services.TryAddSingleton<IHostEnvironment>(new HostingEnvironment { EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") });

var secretFilePath = "Secured/secrets.json";

builder.Services.AddDbContext<DbContextApi>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MAIN_DB")));
builder.Services.AddLogging(configuration);
builder.Services.AddScoped<IADB2CRepository, ADB2CRepository>();

builder.Services.AddSecretConfiguration<AzureAdB2C>(configuration, secretFilePath);
builder.Services.AddSecretConfiguration<AuthorizationSettings>(configuration, secretFilePath);
builder.Services.AddSecretConfiguration<MailSettings>(configuration, secretFilePath);

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

