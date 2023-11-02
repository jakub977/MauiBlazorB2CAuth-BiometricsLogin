using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Infrastructure;
using Principal.Telemedicine.Shared.Logging;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Penelope API", Version = "V1" });
    config.OperationFilter<TraceHeaderParameter>();
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
//Trace requests
app.UseMiddleware<TracingMiddleware>();
//Login requests
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
