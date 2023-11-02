﻿using Azure.Identity;
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
    private string? _applicationDomain = "";
    private readonly string _logName = "ADB2CRepository";

    public ADB2CRepository(IConfiguration configuration, ILogger<ADB2CRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _tenantId = _configuration["AzureAdB2C:TenantId"];
        _clientId = _configuration["AzureAdB2C:ClientId"];
        _clientSecret = _configuration["AzureAdB2C:ClientSecret"];
        _applicationDomain = _configuration["AzureAdB2C:B2CApplicationDomain"];
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateUserAsyncTask(Customer customer)
    {
        bool ret = false;
        string logHeader = _logName + ".UpdateUserAsyncTask:";
        try
        {
            if (customer.Id <= 0)
            {
                _logger.LogWarning("{0} Can't update user with email '{1}' and Id {2} - user is new", logHeader, customer.Email, customer.Id);
                return ret;
            }

            // UPN ukládáme jako email převedený na Base64 + aplikační doména
            string searchedUPN = CreateUPN(customer.Email);

            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{searchedUPN}'";
            });

            if (result == null || result.Value == null || result.Value.Count == 0)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' not found", logHeader, customer.Email);
                return ret;
            }

            if (result.Value.Count > 1)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' exists more than once: {1}", logHeader, customer.Email, result.Value.Count);
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
    public async Task<bool> InsertUserAsyncTask(Customer customer)
    {
        bool ret = false;
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
                return ret;
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
            ret = true;

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', Email: '{1}', Id: {2} created succesfully", logHeader, customer.FriendlyName, customer.Email, customer.Id);
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
    public async Task<bool> DeleteUserAsyncTask(Customer customer)
    {
        bool ret = false;
        string logHeader = _logName + ".DeleteUserAsyncTask:";

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

            if (result == null || result.Value == null || result?.Value?.Count != 1)
            {
                _logger.LogWarning("{0} ADB2C returned: User with email '{0}' not found", logHeader, customer.Email);
                return ret;
            }

            string? userId = result?.Value[0].Id;

            if (userId != null)
            {
                await GetClient().Users[userId].DeleteAsync();
                ret = true;
            }

            _logger.LogDebug("{0} ADB2C returned: OK, user '{0}', Email: '{1}', Id: {2} deleted succesfully", logHeader, customer.FriendlyName, customer.Email, customer.Id);
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
    public async Task<int> CheckUserAsyncTask(Customer customer)
    {
        int ret = -1;
        string logHeader = _logName + ".CheckUserAsyncTask:";

        try
        {
            // UPN je email převedený na Base64 + aplikační doména
            string searchedUPN = CreateUPN(customer.Email);

            // kontrola na existující účet
            var result = await GetClient().Users.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                requestConfiguration.QueryParameters.Filter = $"userPrincipalName eq '{searchedUPN}'";
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
    /// Vytvoří UPN z emailu. UPN je tvořeno emailem kódovaným jako Base64 a přidáním "@aplikační doména".
    /// V Base64 řetězci je pak nahrazen znak "=" znakem "_", jinak by nešlo UPN uložit v ADB2C
    /// </summary>
    /// <param name="email">email</param>
    /// <returns>UPN</returns>
    private string CreateUPN(string email)
    {
        return Base64Encode(email).Replace("=", "_") + "@" + _applicationDomain;
    }

    /// <summary>
    /// Vrátí email zakódovaný v UPN
    /// </summary>
    /// <param name="upn">UPN</param>
    /// <returns>email</returns>
    private string GetEmailFromUPN(string upn)
    {
        string temp = upn.Replace("\"@\"" + _applicationDomain, "");
        temp = temp.Replace("_", "=");
        return Base64Decode(temp);
    }

    #endregion
}

