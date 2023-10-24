namespace Principal.Telemedicine.Shared.Cache;

/// <summary>
/// Nastavení in-process keše.
/// </summary>

public class MemoryCacheOptions
{
    /// <summary>
    /// Defaultní parametry pro zápis položky do keše.
    /// </summary>
    public CacheEntryOptions DefaultCacheEntryOption { get; set; }
}

