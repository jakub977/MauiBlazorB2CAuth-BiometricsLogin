using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Principal.Telemedicine.Shared.Interfaces;

public class GraphAPI: IGraphAPI
{
    private readonly ILogger _logger;
    private readonly string _logName = "GraphAPI";


    public GraphAPI(ILogger<GraphAPI> logger)
    {
        _logger = logger;
    }

    public string GetEmailRequestBody(string recipientsEmail, string messageSubject, string messageBody)
    {
        string logHeader = _logName + ".GetEmailRequestBody:";
        string jsonMessage = string.Empty; 

        try
        {
            var message = new Dictionary<string, object>()
                {
                    {"message", new Dictionary<string, object>()
                        {
                            {"subject", messageSubject},
                            {"body", new Dictionary<string, object>()
                                {
                                    {"contentType", "Text"},
                                    {"content", messageBody}
                                }
                            },
                            {"toRecipients", new object[]
                               {
                                new Dictionary<string, object>()
                                {
                                    {"emailAddress", new Dictionary<string, object>()
                                        {
                                            {"address", recipientsEmail}
                                        }
                                    }
                                }
                               }
                            },
                         }
                    },
                    {"saveToSentItems", "true"}
                };

                jsonMessage = JsonSerializer.Serialize(message);
               
                if (string.IsNullOrEmpty(jsonMessage))
                {
                    _logger.LogDebug($"{logHeader} Request body of email to user '{recipientsEmail}' was not prepared.");
                }
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} returned: Request body of email to user '{recipientsEmail}', Error: {errMessage}");
        }

        return jsonMessage;
    }

    public async Task<string?> GetAccessTokenAsync(HttpClient client, string tenantId, FormUrlEncodedContent requestContent )
    {
        string logHeader = _logName + ".GetAccessTokenAsync:";
        string accessToken = string.Empty;
        string tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

        try
        {
             HttpResponseMessage response = await client.PostAsync(tokenEndpoint, requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            accessToken = tokenResponse.GetProperty("access_token").GetString();

            _logger.LogDebug($"{logHeader} AD returned: OK, token returned succesfully");
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} AD returned: Error: {errMessage}");
        }
        return accessToken;
    }

}
