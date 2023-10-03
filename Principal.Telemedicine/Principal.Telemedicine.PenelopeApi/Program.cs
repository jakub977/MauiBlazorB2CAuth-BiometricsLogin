using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.DataConnectors.Contexts;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbContextApi>(options => options.UseLazyLoadingProxies().
UseSqlServer(builder.Configuration.GetConnectionString("MAIN_DB")));

builder.Services.AddDbContext<DbContextApi>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TMWorkstore")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsLocalHosted())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
