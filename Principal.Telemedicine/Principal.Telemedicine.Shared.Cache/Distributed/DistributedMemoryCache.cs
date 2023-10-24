

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Interfaces;
using ME = Microsoft.Extensions.Caching.Memory;

namespace Principal.Telemedicine.Shared.Cache;

/// <inheritdoc/>
public class DistributedMemoryCache : IDistributedCache
{
    private readonly ILogger _logger;

    private readonly ME.IMemoryCache _cache;

    private readonly DistributedCacheOptions _options;

    public DistributedMemoryCache(ME.IMemoryCache cache, IOptions<DistributedCacheOptions> options, ILogger<DistributedMemoryCache> logger)
    {
        _logger = logger;
        _cache = cache;
        _options = options.Value;
    }

    /// <inheritdoc />
    public IBaseCacheEntryOptions Add<T>(string key, T value, IBaseCacheEntryOptions? options = null)
    {
        var opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        var entry = _cache.Set(key, value, (MemoryCacheEntryOptions)opt);
        return opt;
    }

    /// <inheritdoc />
    public async Task<IBaseCacheEntryOptions> AddAsync<T>(string key, T value, CancellationToken cancellationToken = default, IBaseCacheEntryOptions? options = null)
    {
        var opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        var entry = _cache.Set(key, value, (MemoryCacheEntryOptions)opt);
        return await Task.FromResult(opt);
    }

    /// <inheritdoc />
    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    /// <inheritdoc />
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_cache.Get<T>(key));
    }

    /// <inheritdoc />
    public bool TryRemove(string key)
    {
        object value;
        if (_cache.TryGetValue(key, out value))
        {
            _cache.Remove(key);
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public async Task<bool> TryRemoveAsync(string key)
    {
        object value;
        if (_cache.TryGetValue(key, out value))
        {
            _cache.Remove(key);
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }
}

