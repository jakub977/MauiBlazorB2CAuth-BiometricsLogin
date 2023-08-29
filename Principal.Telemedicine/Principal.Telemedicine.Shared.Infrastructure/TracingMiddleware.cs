using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Principal.Telemedicine.Shared.Constants;

namespace Principal.Telemedicine.Shared.Infrastructure;

/// <summary>
/// Uloží do paměti v rámci requestu informaci o aktuálním trace.
/// </summary>
public class TracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<TracingMiddleware> _logger;
    private readonly TmAppConfiguration _appConfig;

    public TracingMiddleware(RequestDelegate next, ILogger<TracingMiddleware> logger, IMemoryCache cache, IOptions<TmAppConfiguration> appConfig)
    {
        _next = next;
        _logger = logger;
        _cache = cache;
        if (appConfig == null) logger.LogError("App configuration is not presents in appsettings.json");
        _appConfig = appConfig.Value;
    }

    /// <summary>
    /// V rámci requestu se kontroluje header a pokud již obsahuje informaci o trace, tak jí zapíše do paměti. 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        string key = $"{HeaderKeysConst.TRACE_KEY}_{context.TraceIdentifier}";
        
        string actualValue = null ;
        try
        {
            if (context.Request.Headers.ContainsKey(HeaderKeysConst.TRACE_KEY))
            {

                actualValue = Utils.StringUtils.JoinPath(context.Request.Headers[HeaderKeysConst.TRACE_KEY].First(), _appConfig.IdentificationId, _appConfig.IdentificationDelimeter);
                _cache.Set<string?>(key, actualValue, new TimeSpan(0, 20, 0));

            }
            else
            {
                actualValue = _appConfig.IdentificationId;
                _logger.LogWarning($"Parameter {HeaderKeysConst.TRACE_KEY} in call header not found");
            }

            if (context.Response.Headers.TryGetValue(HeaderKeysConst.TRACE_KEY, out StringValues headerVal))
            {
                context.Response.Headers.Remove(HeaderKeysConst.TRACE_KEY);
            }

        }
        catch (Exception ex) { _logger.LogError(ex.ToString()); }
        finally
        {
          
            //zpátky posílám 
            context.Response.Headers.Add(HeaderKeysConst.TRACE_KEY, actualValue);
            await _next.Invoke(context);
        }
    }
}
