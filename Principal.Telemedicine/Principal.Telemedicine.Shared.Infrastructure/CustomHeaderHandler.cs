using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Principal.Telemedicine.Shared.Contants;

namespace Principal.Telemedicine.Shared.Infrastructure;
public class CustomHeaderHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private readonly IHttpContextAccessor _contextAccessor;

    public CustomHeaderHandler(IMemoryCache cache, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _cache = cache;
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
        return await base.SendAsync(request, cancellationToken);
    }
}