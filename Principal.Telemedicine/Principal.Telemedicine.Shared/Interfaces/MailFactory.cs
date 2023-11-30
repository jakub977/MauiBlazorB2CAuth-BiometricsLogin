using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Principal.Telemedicine.Shared.Interfaces;

public class MailFactory : IMailFactory
{
    private readonly ILogger _logger;
    private readonly IGraphAPI _graphAPI;
    private readonly MailSettings _mailSettings;
    private readonly string _logName = "MailFactory";

    public MailFactory(ILogger<MailFactory> logger, IOptions<MailSettings> mailSettings, IGraphAPI graphAPI)
    {
        _logger = logger;
        _mailSettings = mailSettings.Value;
        _graphAPI = graphAPI;
    }

    /// <inheritdoc/>
    public async Task<bool> SendEmailAsyncTask(string recipientsEmail, string messageSubject, string messageBody)
    {
        bool ret = false;
        string logHeader = _logName + ".SendEmailAsyncTask:";

        HttpResponseMessage sendMailResponse = new HttpResponseMessage();
        bool result = false;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _mailSettings.ClientId),
                    new KeyValuePair<string, string>("client_secret", _mailSettings.ClientSecret),
                    new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),

             });

                // retrieve access token
                string? accessToken = await _graphAPI.GetAccessTokenAsync(client, _mailSettings.TenantId, requestContent);

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning($"{logHeader} AD returned: Access token not found");
                    return result;
                }

                //sending email
                string sendMailEndpoint = $"https://graph.microsoft.com/v1.0/users/{_mailSettings.ObjectId}/sendMail";

                var jsonMessage = _graphAPI.GetEmailRequestBody(recipientsEmail, messageSubject, messageBody);

                if (jsonMessage == null)
                {
                    _logger.LogWarning($"{logHeader} Request body for sending email to user '{recipientsEmail}' is empty'");
                    return result;
                }
                var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                // post request to send the email
                var request = new HttpRequestMessage(HttpMethod.Post, sendMailEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = content;

                sendMailResponse = await client.SendAsync(request);
                string sendMailResponseContent = await sendMailResponse.Content.ReadAsStringAsync();
                if (sendMailResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"{logHeader} AD returned: OK, email to user: '{recipientsEmail}' sent succesfully");
                    result = true;
                }
                else
                {
                    _logger.LogWarning($"{logHeader} AD returned: Email was not send to user '{recipientsEmail}'");
                }
            }
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} AD returned: User: '{recipientsEmail}', Error: {errMessage}");
        }

        return result;
    }
}
