using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Constants;
using Principal.Telemedicine.Shared.Interfaces;

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

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        return base.Send(request, cancellationToken);
    }

    /// <summary>
    /// Add into call headers tracing header that take from IMemmoryCache. Registered TracingMiddleware is requiered.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        //Authorize Header
        if (_contextAccessor?.HttpContext != null && _contextAccessor.HttpContext.Request.Headers.ContainsKey(HeaderKeysConst.AUTHORIZE_KEY))
        {
            try
            {
                var stream = _contextAccessor.HttpContext.Request.Headers[HeaderKeysConst.AUTHORIZE_KEY];
                if (request.Headers.Contains(HeaderKeysConst.AUTHORIZE_KEY)) request.Headers.Remove(HeaderKeysConst.AUTHORIZE_KEY);
                request.Headers.Add(HeaderKeysConst.AUTHORIZE_KEY, stream.FirstOrDefault());
            }
            catch (Exception ex)
            { _logger.LogError($"Problem in add authorization header during call by HttpClient >> {ex.ToString()}"); }
        }






         string key = $"{HeaderKeysConst.TRACE_KEY}_{_contextAccessor.HttpContext.TraceIdentifier}";
        string val = await _cache.GetAsync<string>(key, cancellationToken);
        if (!string.IsNullOrWhiteSpace(val))
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