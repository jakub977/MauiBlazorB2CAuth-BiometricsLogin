using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Contants;
using System.Text;



namespace Principal.Telemedicine.Shared.Logging;
/// <summary>
/// Middleware pro zápis requestů a responsů do Customlog (db). 
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    private string _traceMethod;
    private string _tracePath;
    private string _sessionTraceCall = Guid.NewGuid().ToString();

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Zpracování požadavku a odpovědi - zalogování do DB.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        await LogRequest(context);

        var originalResponseBody = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            await _next.Invoke(context);

            await LogResponse(context, responseBody, originalResponseBody);
        }
    }

    private async Task LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
    {
        var responseContent = new StringBuilder();
        responseContent.AppendLine("=== Response Info ===");
        string traceResponse = string.Empty;
    
        responseContent.AppendLine("-- headers");
        foreach (var (headerKey, headerValue) in context.Response.Headers)
        {
            if (headerKey.Equals(HeaderKeysConst.TRACE_KEY, StringComparison.InvariantCulture)) traceResponse = headerValue;
            responseContent.AppendLine($"header = {headerKey}    value = {headerValue}");
        }

        responseContent.AppendLine("-- body");
        responseBody.Position = 0;
        var content = await new StreamReader(responseBody).ReadToEndAsync();
        responseContent.AppendLine($"body = {content}");
        responseBody.Position = 0;
        await responseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;

        _logger.LogCustom(Enumerators.CustomLogLevel.Audit, "INPUT RESPONSE", $"[{_traceMethod}] {_tracePath}", "INPUT CALL", _sessionTraceCall, responseContent.ToString(), traceResponse);
    }

    private async Task LogRequest(HttpContext context)
    {
        var requestContent = new StringBuilder();
        string traceRequest = string.Empty;
        _traceMethod = context.Request.Method.ToUpper();
        _tracePath = context.Request.Path;
        requestContent.AppendLine("=== Request Info ===");
        requestContent.AppendLine($"method = {_traceMethod}");
        requestContent.AppendLine($"path = {_tracePath}");

        requestContent.AppendLine("-- headers");
        foreach (var (headerKey, headerValue) in context.Request.Headers)
        {
            requestContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            if (headerKey.Equals(HeaderKeysConst.TRACE_KEY, StringComparison.InvariantCulture)) traceRequest = headerValue;
        }

        requestContent.AppendLine("-- body");
        context.Request.EnableBuffering();
        var requestReader = new StreamReader(context.Request.Body);
        var content = await requestReader.ReadToEndAsync();
        requestContent.AppendLine($"body = {content}");

        _logger.LogCustom(Enumerators.CustomLogLevel.Audit, "INPUT REQUEST", $"[{_traceMethod}] {_tracePath}", "INPUT CALL", _sessionTraceCall, requestContent.ToString(), traceRequest);
        context.Request.Body.Position = 0;
    }
}
