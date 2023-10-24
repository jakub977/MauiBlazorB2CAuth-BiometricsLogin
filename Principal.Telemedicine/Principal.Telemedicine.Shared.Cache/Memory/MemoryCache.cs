

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Principal.Telemedicine.Shared.Interfaces;
using ME = Microsoft.Extensions.Caching.Memory;

namespace Principal.Telemedicine.Shared.Cache;

/// <inheritdoc />
public class MemoryCache : Interfaces.IMemoryCache
{
    private readonly ME.IMemoryCache _cache;

    private readonly MemoryCacheOptions _options;

    private readonly ILogger _logger;

    private static CancellationTokenSource clearCacheToken;

    public MemoryCache(ME.IMemoryCache cache, IOptions<MemoryCacheOptions> options, ILogger<MemoryCache> logger)
    {
        _cache = cache;
        _options = options.Value;
        _logger = logger;

        clearCacheToken = new CancellationTokenSource();
    }

    /// <inheritdoc />
    public IBaseCacheEntryOptions Add<T>(string key, T value, IBaseCacheEntryOptions? options = null)
    {
        CacheEntryOptions opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        var cachedItem = _cache.GetOrCreate<T>(
            key,
            cacheEntry =>
            {
                cacheEntry.SetOptions((ME.MemoryCacheEntryOptions)opt);
                cacheEntry.AddExpirationToken(new CancellationChangeToken(clearCacheToken.Token));
                cacheEntry.SetValue(value);
                return value;
            });
        return opt;
    }

    /// <inheritdoc />
    public async Task<IBaseCacheEntryOptions> AddAsync<T>(string key, T value, CancellationToken cancellationToken = default, IBaseCacheEntryOptions? options = null) //HOTOVO
    {
        CacheEntryOptions opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        await _cache.GetOrCreateAsync<T>(
            key,
            cacheEntry =>
            {
                cacheEntry.SetOptions((MemoryCacheEntryOptions)opt);
                cacheEntry.AddExpirationToken(new CancellationChangeToken(cancellationToken));
                cacheEntry.AddExpirationToken(new CancellationChangeToken(clearCacheToken.Token));
                cacheEntry.SetValue(value);
                return Task.FromResult(value);
            });

        return opt;
    }

    /// <inheritdoc />
    public void Clear()
    {
        if (clearCacheToken != null && !clearCacheToken.IsCancellationRequested && clearCacheToken.Token.CanBeCanceled)
        {
            clearCacheToken.Cancel();
            clearCacheToken.Dispose();
        }

        clearCacheToken = new CancellationTokenSource();
    }

    /// <inheritdoc />
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        if (clearCacheToken != null && !clearCacheToken.IsCancellationRequested && clearCacheToken.Token.CanBeCanceled)
        {
            clearCacheToken.Cancel();
            clearCacheToken.Dispose();
        }

        clearCacheToken = new CancellationTokenSource();
    }

    /// <inheritdoc />
    public T Get<T>(string key)
    {
        var value = _cache.Get<T>(key);
        return value;
    }

    /// <inheritdoc />
    public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = _cache.Get<T>(key);
        return Task.FromResult(value);
    }

    /// <inheritdoc />
    public bool TryRemove(string key)
    {
        if (_cache.TryGetValue(key, out var mc))
        {
            _cache.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc />
    public Task<bool> TryRemoveAsync(string key)
    {
        if (_cache.TryGetValue(key, out var mc))
        {
            _cache.Remove(key);
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }
}
