using Microsoft.Extensions.Hosting;

namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Pomocná třída pro získání prostředí, ve kterém běží aplikace
/// </summary>
public static class ExtensionsForIHostEnvironment
{
    internal static IEnumerable<string> ENVIRONMENT_NAMES_LOCAL = new string[1] { "local" };
    internal static IEnumerable<string> ENVIRONMENT_NAMES_OUT = new string[4] { "development", "testing", "staging", "production" };
  
    /// <summary>
    /// Metoda vrací true v případě, že aplikace běží v lokálním prostředí
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_LOCAL.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Metoda vrací true v případě, že neběží v lokálním prostředí. 
    /// </summary>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static bool IsNotLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_LOCAL.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase);
    }
}
