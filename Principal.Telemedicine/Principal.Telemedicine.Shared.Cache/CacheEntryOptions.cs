using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Principal.Telemedicine.Shared.Interfaces;

namespace Principal.Telemedicine.Shared.Cache;

/// <inheritdoc />
public class CacheEntryOptions : IBaseCacheEntryOptions
{
    /// <inheritdoc />
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    /// <inheritdoc />
    public TimeSpan? SlidingExpiration { get; set; }

    /// <inheritdoc />
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Implicitní konvertor pro DistributedCacheEntryOptions.
    /// </summary>
    /// <param name="options">Vlastní instance třídy CacheEntryOptions</param>
    public static implicit operator DistributedCacheEntryOptions(CacheEntryOptions options) =>
        new()
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration
        };

    /// <summary>
    /// Implicitní konvertor pro MemoryCacheEntryOptions.
    /// </summary>
    /// <param name="options">Instance <see cref="CacheEntryOptions"/>.</param>
    public static explicit operator MemoryCacheEntryOptions(CacheEntryOptions options) =>
        new()
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration
        };
}
