using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <inheritdoc/>
public class ADB2CRepository : IADB2CRepository
{

    private readonly ILogger _logger;
    private readonly AzureAdB2C _adB2C;

    private string? _tenantId = "";
    private string? _clientId = "";
    private string? _clientSecret = "";
    private string? _extensionClientId = "";
    private string? _applicationDomain = "";
    private bool _allowWebApiToBeAuthorizedByACL = false;
    private readonly string _logName = "ADB2CRepository";
                            
    public ADB2CRepository(ILogger<ADB2CRepository> logger, IOptions<AzureAdB2C> adB2C)
    {
        _logger = logger;
        _adB2C = adB2C.Value;
        _tenantId = _adB2C.TenantId;
        _clientId = _adB2C.ClientId;
        _clientSecret = _adB2C.ClientSecret;
        _extensionClientId = _adB2C.B2cExtensionAppClientId;
        _applicationDomain = _adB2C.B2CApplicationDomain;
        _allowWebApiToBeAuthorizedByACL = _adB2C.AllowWebApiToBeAuthorizedByACL;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateUserAsyncTask(Customer customer)
    {
        int ret = -1;
        string logHeader = _logName + ".UpdateUserAsyncTask:";
        try
        {
            if (customer.Id <= 0)
            {
                _logger.LogWarning("{0} Can't update user UPN '{1}' and Id {2} - user is new", logHeader, customer.GlobalId, customer.Id);
                return -15;
            }

            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{customer.GlobalId}'";
            });

            if (result == null || result.Value == null || result.Value.Count == 0)
            {
                _logger.LogWarning("{0} ADB2C returned: User UPN '{0}' not found", logHeader, customer.GlobalId);
                return -16;
            }

            if (result.Value.Count > 1) // k tomuto zřejme nedojde pokud hledáme podle UPN
            {
                _logger.LogWarning("{0} ADB2C returned: User UPN '{0}' exists more than once: {1}", logHeader, customer.GlobalId, result.Value.Count);
                return -17;
            }

            User userFound = result.Value[0];
            userFound.Surname = customer.LastName;
            userFound.GivenName = customer.FirstName;
            userFound.MobilePhone = customer.TelephoneNumber;
            userFound.AccountEnabled = customer.Active;
            userFound.DisplayName = customer.FriendlyName;
            userFound.Mail = customer.Email;
            if (customer.CityId > 0 && customer.City != null)
                userFound.City = customer.City.Name;

            await GetClient().Users[$"{userFound.Id}"].PatchAsync(userFound);
            ret = 1;

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', UPN: '{1}', Id: {2} updated succesfully", logHeader, customer.FriendlyName, customer.GlobalId, customer.Id);
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError("{0} ADB2C returned: User: '{0}', Error: {1}", logHeader, customer.GlobalId, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<int> InsertUserAsyncTask(Customer customer)
    {
        int ret = -1;
        string logHeader = _logName + ".InsertUserAsyncTask:";

        try
        {
            // UPN ukládáme jako email převedený na Base64 + aplikační doména
            string searchedUPN = CreateUPN(customer.Email);

            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{searchedUPN}'";
            });

            if (result != null && result.Value != null && result?.Value?.Count > 0)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' already exists", logHeader, customer.Email);
                return -18;
            }

            User userNew = new User();
            userNew.Surname = customer.LastName;
            userNew.GivenName = customer.FirstName;
            userNew.MobilePhone = customer.TelephoneNumber;
            userNew.AccountEnabled = customer.Active;
            userNew.DisplayName = customer.FriendlyName;
            userNew.Mail = customer.Email;
            if (customer.CityId > 0 && customer.City != null)
                userNew.City = customer.City.Name;

            userNew.PasswordPolicies = "DisablePasswordExpiration";
            userNew.PasswordProfile = new PasswordProfile();
            userNew.PasswordProfile.ForceChangePasswordNextSignIn = true;
            userNew.PasswordProfile.Password = customer.Password;

            userNew.UserPrincipalName = searchedUPN;

            userNew.Identities = new List<ObjectIdentity>
            {
                new ObjectIdentity() { SignInType = "emailAddress", Issuer = _applicationDomain, IssuerAssignedId = customer.Email }
            };

            var createdUser = await GetClient().Users.PostAsync(userNew);
            ret = 1;

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', Email: '{1}', Id: {2} created succesfully", logHeader, customer.FriendlyName, customer.Email, customer.Id);
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }

            if (errMessage.Contains("proxyAddresses already exists"))
                ret = -19;

            _logger.LogError("{0} ADB2C returned: User: '{0}', Error: {1}", logHeader, customer.Email, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUserAsyncTask(Customer customer)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteUserAsyncTask:";

        try
        {
            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{customer.GlobalId}'";
            });

            if (result == null || result.Value == null || result?.Value?.Count != 1)
            {
                _logger.LogWarning("{0} ADB2C returned: User with UPN '{0}' not found", logHeader, customer.GlobalId);
                return ret;
            }

            string? userId = result?.Value[0].Id;

            if (userId != null)
            {
                await GetClient().Users[userId].DeleteAsync();
                ret = true;
            }

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', UPN: '{1}', Id: {2} deleted succesfully", logHeader, customer.FriendlyName, customer.GlobalId, customer.Id);
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError("{0} ADB2C returned: User: '{0}', Error: {1}", logHeader, customer.Email, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<Customer?> GetUserByObjectIdAsyncTask(string objectId)
    {
        string logHeader = _logName + ".GetUserByObjectIdAsyncTask:";
        Customer cus = new Customer();

        try
        {
            // nalezení uživatele pomocí id
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "displayName", "mail", "userPrincipalName", "mobilePhone" };
                requestConfiguration.QueryParameters.Filter = $"id eq '{objectId}'";
            });

            if (result == null || result.Value == null || result?.Value?.Count != 1)
            {
                _logger.LogWarning($"{logHeader} ADB2C returned: User with object id '{objectId}' not found");
                return cus;
            }

            string? mail = result?.Value[0].Mail;
            string? upn = result?.Value[0].UserPrincipalName;
            string? telephoneNumber = result?.Value[0].MobilePhone;
            string? displayName = result?.Value[0].DisplayName;

            // pokud není vyplněn mail, získáme ho z userPrincipalName
            if (mail == null)
            {
                if (upn != null)
                {
                    mail = GetEmailFromUPN(upn);
                    if (string.IsNullOrEmpty(mail))
                        _logger.LogWarning($"'{logHeader}' ADB2C returned: User'{cus.FriendlyName}', Email: '{cus.Email}, Object id '{objectId}' not found by mail");
                    return cus;
                }

                else
                {
                    _logger.LogWarning($"'{logHeader}' ADB2C returned: User'{cus.FriendlyName}', Email: '{cus.Email}, Object id '{objectId}' not found by mail");
                    return cus;
                }
            }

            cus.Email = mail;
            cus.FriendlyName = displayName;
            cus.TelephoneNumber = telephoneNumber;

            _logger.LogDebug($"'{logHeader}' ADB2C returned: User '{cus.FriendlyName}', Email: '{cus.Email}', ObjectId: '{objectId}' succesfully");
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"'{logHeader}' ADB2C returned: User ObjectId: '{objectId}', Error: '{errMessage}'");
        }

        return cus;
    }

    /// <inheritdoc/>
    public async Task<bool> SendEmailAsyncTask(string recipientsEmail, string messageBody, string messageTitle)
    {
        bool ret = false;
        string logHeader = _logName + ".SendEmailAsyncTask:";
        try
        {
            // UPN ukládáme jako email převedený na Base64 + aplikační doména
            string searchedUPN = CreateUPN(recipientsEmail);

            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{searchedUPN}'";
            });

            if (result == null || result.Value == null || result.Value.Count == 0)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' not found", logHeader, recipientsEmail);
                return ret;
            }

            if (result.Value.Count > 1)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' exists more than once: {1}", logHeader, recipientsEmail, result.Value.Count);
                return ret;
            }

           //     Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody requestbody = new()
           //     {
           //         Message = new Message ()
           //         {
           //             Subject = messageTitle,
           //             Body = new ItemBody
           //             {
           //                 ContentType = BodyType.Text,
           //                 Content = messageBody
           //             },
           //             ToRecipients = new List<Recipient>()
           //             {
           //                 new Recipient
           //                 {
           //                     EmailAddress = new EmailAddress
           //                     {
           //                         Address = recipientsEmail
           //                     }
           //                 }
           //             }
            
           //         },

           //         SaveToSentItems = false
           //     };


           // string objectId = "476d377e-27fc-41ef-ba6c-be2079e0df2a";


           //await GetClient().Users[objectId].
           //     SendMail.PostAsync(requestbody,
           //     requestConfiguration =>
           //     {
           //         requestConfiguration.Headers.Add("Prefer", "outlook.body-content-type=\"text\"");
           //     });

            ret = true; 

            _logger.LogDebug($"{logHeader} ADB2C returned: OK, Email to User: '{recipientsEmail}' sent succesfully");
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"{logHeader} ADB2C returned: User: '{recipientsEmail}', Error: {errMessage}");
        }

        return ret;
    }

    /// <inheritdoc/>
    public async Task<int> CheckUserAsyncTask(Customer customer)
    {
        int ret = -1;
        string logHeader = _logName + ".CheckUserAsyncTask:";

        try
        {
            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{customer.GlobalId}'";
            });

            // existuje účet
            if (result != null && result.Value != null && result?.Value?.Count > 0)
                ret = 1;
            else 
                ret = 0;

            _logger.LogDebug("{0} ADB2C returned: {0}, user '{1}', Email: '{2}', Id: {3}", logHeader, ret, customer.FriendlyName, customer.Email, customer.Id);
        }
        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError("{0} ADB2C returned: User: '{0}', Error: {1}", logHeader, customer.Email, errMessage);
        }

        return ret;
    }

    /// <inheritdoc/>
    public string CreateUPN(string email)
    {
        return Base64Encode(email).Replace("=", "_") + "@" + _applicationDomain;
    }

    /// <inheritdoc/>
    public string? GetApplicationDomain()
    {
        return _applicationDomain;
    }

    #region Private methods

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

    /// <summary>
    /// Převede text na Base64 řetězec
    /// </summary>
    /// <param name="text">Taxt k převodu</param>
    /// <returns>Base64</returns>
    private static string Base64Encode(string text)
    {
        var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(textBytes);
    }

    /// <summary>
    /// Převede Base64 řetězec na test
    /// </summary>
    /// <param name="base64">Base64 řetězec</param>
    /// <returns>Txext</returns>
    private static string Base64Decode(string base64)
    {
        var base64Bytes = System.Convert.FromBase64String(base64);
        return System.Text.Encoding.UTF8.GetString(base64Bytes);
    }

    /// <summary>
    /// Vrátí email zakódovaný v UPN
    /// </summary>
    /// <param name="upn">UPN</param>
    /// <returns>email</returns>
    private string GetEmailFromUPN(string upn)
    {
        string temp = string.Empty;

        try
        {
            string? applicationDomain = _applicationDomain;
            temp = upn.Replace($"@{applicationDomain}", "").Replace("__", "==");
            return Base64Decode(temp);
        }

        catch (Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage += " " + ex.InnerException.Message;
            }
            _logger.LogError($"GetEmailFromUPN returned: User with upn: '{upn}', Error: {errMessage}");
        }

        return temp;

    }

    #endregion
}

