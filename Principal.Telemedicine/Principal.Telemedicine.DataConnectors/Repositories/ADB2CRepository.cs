using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class ADB2CRepository : IADB2CRepository
{

    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    private string? _tenantId = "";
    private string? _clientId = "";
    private string? _clientSecret = "";
    private string? _extensionClientId = "";
    private readonly string _logName = "UserApiController";

    public ADB2CRepository(IConfiguration configuration, ILogger<ADB2CRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _tenantId = _configuration["AzureAdB2C:TenantId"];
        _clientId = _configuration["AzureAdB2C:ClientId"];
        _clientSecret = _configuration["AzureAdB2C:ClientSecret"];
        _extensionClientId = _configuration["AzureAdB2C:B2cExtensionAppClientId"];
    }

    //TO-DO svázat uživatele přes nějaké pole (jiné než email, přes username to nejde)
    /// <inheritdoc/>
    public async Task<bool> UpdateUserAsyncTask(Customer customer)
    {
        bool ret = false;
        string logHeader = _logName + ".UpdateUser:";
        try
        {
            if (customer.Id <= 0)
            {
                _logger.LogWarning("{0} Can't update user with email '{1}' and Id {2} - user is new", logHeader, customer.Email, customer.Id);
                return ret;
            }

            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"startswith(mail,'{customer.Email}')";
            });

            if (result == null || result.Value == null || result.Value.Count == 0)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' not found", logHeader, customer.Email);
                return ret;
            }

            if (result.Value.Count > 1)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' exists more than onece: {1}", logHeader, customer.Email, result.Value.Count);
                return ret;
            }

            User userFound = result.Value[0];
            userFound.Surname = customer.LastName;
            userFound.GivenName = customer.FirstName;
            userFound.MobilePhone = customer.TelephoneNumber;
            userFound.AccountEnabled = customer.Active;
            userFound.DisplayName = customer.FriendlyName;
            if (customer.CityId > 0 && customer.City != null)
                userFound.City = customer.City.Name;

            await GetClient().Users[$"{userFound.Id}"].PatchAsync(userFound);
            ret = true;

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', Email: '{1}', Id: {2} updated succesfully", logHeader, customer.FriendlyName, customer.Email, customer.Id);
        } 
        catch(Exception ex)
        {
            _logger.LogError("{0} ADB2C returned: User: '{0}', Error: {1}", logHeader, customer.Email, ex.Message);
        }

        return ret;
    }

    /// <summary>
    /// Vytvoří Graph klienta
    /// </summary>
    /// <returns>Graph klient</returns>
    private GraphServiceClient GetClient()
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
        return new GraphServiceClient(clientSecretCredential, scopes);
    }
}

