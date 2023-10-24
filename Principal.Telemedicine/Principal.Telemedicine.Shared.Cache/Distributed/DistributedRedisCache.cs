using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using ME = Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Cache;

/// <summary>
/// 
/// </summary>
public class DistributedRedisCache : IDistributedCache
{
    private readonly ILogger _logger;
    private readonly ME.IDistributedCache _cache;
    private readonly ILogger logger;
    private readonly DistributedCacheOptions _options;

    public DistributedRedisCache(ILogger<DistributedRedisCache> logger, IOptions<DistributedRedisCacheOptions> options, ME.IDistributedCache cache)
    {
        _logger = logger;
        _options = options.Value;
        _cache = cache;
    }

    /// <inheritdoc />
    public IBaseCacheEntryOptions Add<T>(string key, T value, IBaseCacheEntryOptions? options = null)
    {
        var opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        var val = JsonSerializer.Serialize(value, typeof(T));
        var valbyte = Encoding.UTF8.GetBytes(val);
        _cache.Set(key, valbyte, opt);
        return opt;
    }

    /// <inheritdoc />
    public async Task<IBaseCacheEntryOptions> AddAsync<T>(string key, T value, CancellationToken cancellationToken = default, IBaseCacheEntryOptions? options = null)
    {
        var opt = (CacheEntryOptions)options ?? _options.DefaultCacheEntryOption;
        var val = JsonSerializer.Serialize(value, typeof(T));
        var valbyte = Encoding.UTF8.GetBytes(val);
        await _cache.SetAsync(key, valbyte, opt, cancellationToken);

        return options ?? _options.DefaultCacheEntryOption;
    }

    /// <inheritdoc />
    public T Get<T>(string key)
    {
        var retValue = default(T);
        var value = _cache.Get(key);
        if (value != null)
        {
            var encodingValue = Encoding.UTF8.GetString(value);
            retValue = JsonSerializer.Deserialize<T>(encodingValue);
        }
        return retValue;
    }

    /// <inheritdoc />
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var retValue = default(T);
        var value = await _cache.GetAsync(key, cancellationToken);
        if (value != null)
        {
            var encodingValue = Encoding.UTF8.GetString(value);
            retValue = JsonSerializer.Deserialize<T>(encodingValue);
        }
        return retValue;
    }

    /// <inheritdoc />
    public bool TryRemove(string key)
    {

        if (_cache.Get(key) != null)
        {
            _cache.Remove(key);
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> TryRemoveAsync(string key)
    {
        if (await _cache.GetAsync(key) != null)
        {
            await _cache.RemoveAsync(key);
            return true;
        }
        return false;
    }
}
