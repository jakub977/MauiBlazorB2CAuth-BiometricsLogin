using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using MsalAuthInMauiBlazor.Data;
using MsalAuthInMauiBlazor.MsalClient;
using MsalAuthInMauiBlazor;
using System.Reflection;
using CommunityToolkit.Maui;

namespace MsalAuthInMauiBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            var executingAssembly = Assembly.GetExecutingAssembly();

            using var stream = executingAssembly.GetManifestResourceStream("MsalAuthInMauiBlazor.appsettings.json");

            var configuration = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<IPCAWrapper, PCAWrapper>();
            builder.Configuration.AddConfiguration(configuration);


            //Register needed elements for authentication
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();


            return builder.Build();
        }
    }
}