using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Infrastructure;
using Principal.Telemedicine.Shared.Logging;
using Principal.Telemedicine.Shared.Cache;
using Principal.Telemedicine.Shared.Firebase;
using Principal.Telemedicine.Shared.Security;
using Principal.Telemedicine.SharedApi.Controllers;
using Principal.Telemedicine.DataConnectors.Repositories;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.TryAddSingleton<IHostEnvironment>(new HostingEnvironment { EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") });
builder.Services.AddTmDistributedCache(configuration, builder.Environment.IsLocalHosted());
builder.Services.AddSecretConfiguration<DistributedRedisCacheOptions>(configuration, "secrets/secrets.json");
builder.Services.AddSecretConfiguration<TmSecurityConfiguration>(configuration, "secrets/secrets.json");
builder.Services.AddSecretConfiguration<FcmSettings>(configuration, "secrets/secrets.json");

builder.Services.AddScoped<FcmNotificationApiController>();
builder.Services.AddScoped<IFcmNotificationService, FcmNotificationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IADB2CRepository, ADB2CRepository>();
builder.Services.AddScoped<IAppMessageRepository, AppMessageRepository>();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddMicrosoftIdentityWebApi(options =>
        {
            configuration.Bind("AzureAdB2C", options);
        },
        options => { configuration.Bind("AzureAdB2C", options); })
    .EnableTokenAcquisitionToCallDownstreamApi(options =>
    {
        configuration.Bind("AzureAdB2C", options);
        options.LogLevel = Microsoft.Identity.Client.LogLevel.Warning;
    })
    .AddInMemoryTokenCaches();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddTmMemoryCache(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Penelope API", Version = "V1" });
    config.OperationFilter<TraceHeaderParameter>();
    config.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Scheme = "Bearer"
        });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<DbContextApi>(options => options.UseLazyLoadingProxies().
    UseSqlServer(builder.Configuration.GetConnectionString("MAIN_DB")));
builder.Services.AddLogging(configuration);
builder.Services.AddTmInfrastructure(configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsLocalHosted())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Security Middleware
app.UseMiddleware<SecurityMiddleware>();
//Trace requests
app.UseMiddleware<TracingMiddleware>();
//Login requests
app.UseMiddleware<LoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();