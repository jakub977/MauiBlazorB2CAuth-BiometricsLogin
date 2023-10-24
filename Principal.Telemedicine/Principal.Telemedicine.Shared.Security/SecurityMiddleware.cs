using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Principal.Telemedicine.Shared.Security;
public class SecurityMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private const string AUTHORIZATION_HEADER = "Authorization";
    public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    /// <summary>
    /// Zpracování požadavku a odpovědi - načtení autorizační hlavičky.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {

        if(context != null && context.Request.Headers.ContainsKey(AUTHORIZATION_HEADER)) {
            var stream = context.Request.Headers[AUTHORIZATION_HEADER];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            if( tokenS.Claims.Any(m => m.ValueType == "Global ID"))
            {
                var globalId = tokenS.Claims.FirstOrDefault(m => m.ValueType == "Global ID").Value;
                if (!string.IsNullOrWhiteSpace(globalId ))
                {
                    //Volani GetUser
                }
            }
        }

        await _next(context).ConfigureAwait(false);
    }


    }
