using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Api;
using Principal.Telemedicine.Shared.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using Principal.Telemedicine.Shared.Constants;
using Principal.Telemedicine.Shared.Interfaces;
using Principal.Telemedicine.Shared.Cache;

namespace Principal.Telemedicine.Shared.Security;

/// <summary>
/// Třída poskytuje Middleware pro vložení custom uživatele do contextu Identity.
/// </summary>
public class SecurityMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly TmSecurityConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly HttpClient _client;
    private const string CHECK_HEADER_KEY = "ChackUserCall";
    private const string CLAIM_GLOBALID = "extension_GlobalID";

    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger, HttpClient client, IOptions<TmSecurityConfiguration> configuration, IDistributedCache cache )
    {
        _configuration = configuration.Value;
        _client = client;
        _cache = cache;
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

        if (context != null && context.Request.Headers.ContainsKey(Constants.HeaderKeysConst.AUTHORIZE_KEY))
        {
            try
            {
                var stream = context.Request.Headers[Constants.HeaderKeysConst.AUTHORIZE_KEY];

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream.First().Replace("Bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase));
                var tokenS = jsonToken as JwtSecurityToken;
                if (tokenS.Claims.Any(m => m.Type == CLAIM_GLOBALID))
                {

                    var globalId = tokenS.Claims.FirstOrDefault(m => m.Type == CLAIM_GLOBALID).Value;
                    if (!string.IsNullOrWhiteSpace(globalId))
                    {
                        var uriBase = new Uri(_configuration.ResolveUserApiUrl);
                        string url = new Uri(uriBase, $"api/UserApi/GetUser?globalId={globalId}").AbsoluteUri;
                     
                        try
                        {
                            var val = _cache.Get<CompleteUserContract>(string.Format(CacheKeysConst.CACHE_KEY_HTTPUSER_PREFIX, globalId));
                            
                            if (val == null && !context.Request.Headers.ContainsKey(CHECK_HEADER_KEY) && context.GetTmUser() == null)
                            {
                                _client.DefaultRequestHeaders.Add(CHECK_HEADER_KEY, "1");
                                var result = await _client.GetFromJsonAsync<GenericResponse<CompleteUserContract>>(url);
                                if (result.Success)
                                {
                                    val = result.Data;
                                    await _cache.AddAsync(String.Format(CacheKeysConst.CACHE_KEY_HTTPUSER_PREFIX, globalId), val, default(CancellationToken), new CacheEntryOptions() { SlidingExpiration = new(0, 20, 0) });
                                }
                            }
                            context.Items.Add(ItemsKeysConst.CONTEXT_ITEM_USER, val);
                        }
                        catch(Exception ex)
                        {


                        }
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
