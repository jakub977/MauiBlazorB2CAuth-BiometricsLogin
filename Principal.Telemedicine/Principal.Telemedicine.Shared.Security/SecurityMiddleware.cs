using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Principal.Telemedicine.Shared.Infrastructure;


namespace Principal.Telemedicine.Shared.Security;
public class SecurityMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private const string AUTHORIZATION_HEADER = "Authorization";
    private const string CLAIM_GLOBALID = "extension_GlobalID";
    private CancellationTokenSource tokenSource = new CancellationTokenSource();
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
    [Authorize]
    public async Task Invoke(HttpContext context)
    {

        if(context != null && context.Request.Headers.ContainsKey(AUTHORIZATION_HEADER)) {
            var stream = context.Request.Headers[AUTHORIZATION_HEADER];
        
            var handler = new  JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream.First().Replace("Bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase));
            var tokenS = jsonToken as JwtSecurityToken;
            if( tokenS.Claims.Any(m => m.Type == CLAIM_GLOBALID))
            {
                var globalId = tokenS.Claims.FirstOrDefault(m => m.Type == CLAIM_GLOBALID).Value;
                if (!string.IsNullOrWhiteSpace(globalId ))
                {
                    
                  
                    //Volani GetUser
                }
            }
        }

        await _next(context).ConfigureAwait(true);
    }


    }
