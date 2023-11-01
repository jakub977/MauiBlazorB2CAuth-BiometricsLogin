using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;


namespace Principal.Telemedicine.Shared.Security;

/// <summary>
/// Třída poskytuje Middleware pro vložení custom uživatele do contextu Identity.
/// </summary>
public class SecurityMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly HttpClient _client;
    private const string AUTHORIZATION_HEADER = "Authorization";
    private const string CLAIM_GLOBALID = "extension_GlobalID";

    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger, HttpClient client, IOptions<TmSecurityConfiguration>)
    {
        _client = client;
        _next = next;
        _logger = logger;
    }
    /// <summary>
    /// Zpracování požadavku a odpovědi - načtení autorizační hlavičky a vytáhnutí uživatele pro vložení do kontextu.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [Authorize]
    public async Task Invoke(HttpContext context)
    {

        if (context != null && context.Request.Headers.ContainsKey(AUTHORIZATION_HEADER))
        {
            try
            {
                var stream = context.Request.Headers[AUTHORIZATION_HEADER];

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream.First().Replace("Bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase));
                var tokenS = jsonToken as JwtSecurityToken;
                if (tokenS.Claims.Any(m => m.Type == CLAIM_GLOBALID))
                {

                    var globalId = tokenS.Claims.FirstOrDefault(m => m.Type == CLAIM_GLOBALID).Value;
                    if (!string.IsNullOrWhiteSpace(globalId))
                    {


                        //Volani GetUser
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        await _next(context).ConfigureAwait(true);
    }


}
