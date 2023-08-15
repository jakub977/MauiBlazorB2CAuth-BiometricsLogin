using Microsoft.Extensions.Hosting;

namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Pomocná třída pro získání prostředí, ve kterém běží aplikace
/// </summary>
public static class ExtensionsForIHostEnvironment
{
    internal static IEnumerable<string> ENVIRONMENT_NAMES_LOCAL = new string[2] { "local", "development" };
    internal static IEnumerable<string> ENVIRONMENT_NAMES_OUT = new string[3] {  "testing", "staging", "production" };
  
    /// <summary>
    /// Metoda vrací true v případě, že aplikace běží v lokálním prostředí
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_LOCAL.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase) ||
            ENVIRONMENT_NAMES_LOCAL.Contains<string>(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? String.Empty, StringComparer.InvariantCultureIgnoreCase) ;
    }

    /// <summary>
    /// Metoda vrací true v případě, že neběží v lokálním prostředí. 
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsNotLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_OUT.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase) ||
            ENVIRONMENT_NAMES_OUT.Contains<string>(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? String.Empty, StringComparer.InvariantCultureIgnoreCase); ;
    }
}
