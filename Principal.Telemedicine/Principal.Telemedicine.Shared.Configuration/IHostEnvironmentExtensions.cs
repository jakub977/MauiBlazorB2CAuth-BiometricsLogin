using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Configuration;
public static class ExtensionsForIHostEnvironment
{
    internal static IEnumerable<string> ENVIRONMENT_NAMES_LOCAL = new string[1] { "local" };
    internal static IEnumerable<string> ENVIRONMENT_NAMES_OUT = new string[4] { "development", "testing", "staging", "production" };
    public static bool IsLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_LOCAL.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase);
    }
    public static bool IsNotLocalHosted(this IHostEnvironment hostEnvironment)
    {
        return ENVIRONMENT_NAMES_LOCAL.Contains<string>(hostEnvironment.EnvironmentName, StringComparer.InvariantCultureIgnoreCase);
    }
}
