using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Cache;

/// <summary>
/// Nastavení in-process keše.
/// </summary>

public class MemoryCacheOptions
{
    /// <summary>
    /// Defaultní parametry pro zápis položky do keše.
    /// </summary>
    public CacheEntryOptions DefaultCacheEntryOptions { get; set; }
}

