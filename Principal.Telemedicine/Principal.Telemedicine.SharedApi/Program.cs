using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Mapping;
using Principal.Telemedicine.DataConnectors.Repositories;
using Principal.Telemedicine.Shared.Configuration;

using Principal.Telemedicine.Shared.Cache;
using Principal.Telemedicine.Shared.Infrastructure;
using Principal.Telemedicine.Shared.Logging;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Principal.Telemedicine.Shared.Security;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Graph.Models.ExternalConnectors;
using Principal.Telemedicine.Shared.Firebase;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json").Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTmMemoryCache(configuration);


builder.Services.AddAuthentication(x=>
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


builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)
    .AddXmlDataContractSerializerFormatters();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IEffectiveUserRepository, EffectiveUserRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IRoleMemberRepository, RoleMemberRepository>();
builder.Services.AddScoped<IADB2CRepository, ADB2CRepository>();
builder.Services.AddScoped<ISubjectAllowedToOrganizationRepository, SubjectAllowedToOrganizationRepository>();
builder.Services.AddScoped<IFcmNotificationService, FcmNotificationService>();
builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Shared API", Version = "V1" });
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
   // config.OperationFilter<RequiredHeaderParameter>();
});

builder.Services.AddDbContext<DbContextApi>(options => options.UseLazyLoadingProxies().EnableSensitiveDataLogging().
UseSqlServer(builder.Configuration.GetConnectionString("MAIN_DB")));



builder.Services.AddLogging(configuration);
builder.Services.AddTmInfrastructure(configuration);
builder.Services.AddSecretConfiguration<DistributedRedisCacheOptions>(configuration, "secrets/secrets.json");
builder.Services.AddSecretConfiguration<TmSecurityConfiguration>(configuration, "secrets/secrets.json");
builder.Services.AddTmDistributedCache(configuration, builder.Environment.IsLocalHosted());
var app = builder.Build();

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
