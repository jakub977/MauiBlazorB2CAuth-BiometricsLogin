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

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddTmMemoryCache(configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull )
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IEffectiveUserRepository, EffectiveUserRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IRoleMemberRepository, RoleMemberRepository>();
builder.Services.AddScoped<IADB2CRepository, ADB2CRepository>();
builder.Services.AddScoped<ISubjectAllowedToOrganizationRepository, SubjectAllowedToOrganizationRepository>();
builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Shared API", Version = "V1" });
    config.OperationFilter<RequiredHeaderParameter>();
});

builder.Services.AddDbContext<DbContextApi>(options => options.UseLazyLoadingProxies().EnableSensitiveDataLogging().
UseSqlServer(builder.Configuration.GetConnectionString("MAIN_DB")));



builder.Services.AddLogging(configuration);
builder.Services.AddTmInfrastructure(configuration);
builder.Services.AddSecretConfiguration<DistributedRedisCacheOptions>(configuration, "secrets/secrets.json");
builder.Services.AddTmDistributedCache(configuration, builder.Environment.IsLocalHosted());
var app = builder.Build();
 
if (app.Environment.IsLocalHosted())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Trace requests
app.UseMiddleware<TracingMiddleware>();
//Login requests
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
