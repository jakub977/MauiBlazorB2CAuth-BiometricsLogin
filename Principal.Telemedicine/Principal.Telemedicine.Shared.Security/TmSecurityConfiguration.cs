using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Security;
/// <summary>
/// Konfigurační soubor pro nastavení konfigurace middleware
/// </summary>
public class TmSecurityConfiguration
{
    /// <summary>
    /// Url pro UserApi pro načtení konfigurace o uživateli
    /// </summary>
    public string ResolveUserApiUrl  { get; set; }
}
