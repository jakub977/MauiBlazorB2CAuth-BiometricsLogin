using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Constants;

namespace Principal.Telemedicine.Shared.Infrastructure;
/// <summary>
/// HttpClient Handler pro přidávání Http handleru. Podmínkou je přítomný IHttpContextAccessor.
/// </summary>
public class CustomHeaderHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TmAppConfiguration _appConfig;
    private readonly ILogger _logger;

    public CustomHeaderHandler(IMemoryCache cache, IHttpContextAccessor contextAccessor, IOptions<TmAppConfiguration> appConfig, ILogger<CustomHeaderHandler> logger)
    {
        _contextAccessor = contextAccessor;
        _cache = cache;
        if (appConfig?.Value != null)
        {
            _appConfig = appConfig.Value;
        }
        _logger = logger;
    }
    /// <summary>
    /// Add into call headers tracing header that take from IMemmoryCache. Registered TracingMiddleware is requiered.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string key = $"{HeaderKeysConst.TRACE_KEY}_{_contextAccessor.HttpContext.TraceIdentifier}";
        if (_cache.TryGetValue(key, out string? val))
        {
            request.Headers.Add(HeaderKeysConst.TRACE_KEY, val);
        }
        else
        {
            if (_appConfig?.IdentificationId == null)
            {
                _logger.LogWarning("Call has no identification. App configuration is missing.");
            }
            else
            {
                request.Headers.Add(HeaderKeysConst.TRACE_KEY, _appConfig.IdentificationId);
            }
            }
        return await base.SendAsync(request, cancellationToken);
    }
}