using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Principal.Telemedicine.DataConnectors.Mapping;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Repository;
using Principal.Telemedicine.Shared.Logging;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDownstreamWebApi("PatientInfoApiController", builder.Configuration.GetSection("PatientInfoApiControllerScope"))
    .AddInMemoryTokenCaches();   


builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull )
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injection of ApiDbContext Class
builder.Services.AddDbContext<ApiDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("VANDA_TEST")));

builder.Services.AddLogging(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<LoggingMiddleware>();

app.Run();
