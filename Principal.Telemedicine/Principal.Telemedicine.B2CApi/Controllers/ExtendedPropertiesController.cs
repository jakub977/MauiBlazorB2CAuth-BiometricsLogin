using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Principal.Telemedicine.B2CApi.Models;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Configuration;
using System.Text;

namespace Principal.Telemedicine.B2CApi.Controllers;

/// <summary>
/// Kontroler doplňující vlastní přidané atributy při přihlášení uživatele do aplikace prostřednictvím AD.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ExtendedPropertiesController : ControllerBase
{
    private readonly ILogger<ExtendedPropertiesController> _logger;
    private readonly DbContextApi _context;
    private readonly AuthorizationSettings _authsettings;
    private readonly HostBuilderContext _extension;

    public ExtendedPropertiesController(ILogger<ExtendedPropertiesController> logger, DbContextApi context, IOptions<AuthorizationSettings> authsettings, HostBuilderContext extension)
    {
        _logger = logger;
        _context = context;
        _authsettings = authsettings?.Value;
        _extension = extension;
    }

    /// <summary>
    /// Metoda přidává vlastní atributy, jejichž hodnoty získává z dedikované databáze a vrací je jako claimy v odpovědi AD. 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("AddExtendedProperties")]
    public async Task<IActionResult> AddExtendedProperties()
    {
        try
        {
            bool isLocal = _extension.HostingEnvironment.IsLocalHosted();

            var req = Request;

            // check HTTP basic authorization
            if (!Authorize(req, _logger, isLocal, _authsettings))
            {
                _logger.Log(LogLevel.Error, "HTTP basic authentication validation failed.");
                return new UnauthorizedObjectResult("|API_ERROR_1|Authentication validation failed|");
            }

           // get the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.Log(LogLevel.Error, $"Request body: '{requestBody}' ");

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.Log(LogLevel.Error, "Request body is empty.");
                return new BadRequestObjectResult(new ResponseContent("|API_ERROR_2|General error|"));
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data == null)
            {
                _logger.Log(LogLevel.Error, "Deserialization of request body was not successfull.");
                return new BadRequestObjectResult(new ResponseContent("|API_ERROR_2|General error|"));
            }

            if (data.email == null)
            {
                _logger.Log(LogLevel.Error, "Email is mandatory and is empty.");
                return new BadRequestObjectResult(new ResponseContent("|API_ERROR_3|Email is empty|"));
            }

            string email = Convert.ToString(data.email);

            List<ExtendedPropertiesDataModel> dbResult = new List<ExtendedPropertiesDataModel>();

            // call stored procedure in dedicated database
            _logger.Log(LogLevel.Error, $"Volání db s parametrem email: '{email}' ");
            dbResult = _context.ExecSqlQuery<ExtendedPropertiesDataModel>($"dbo.sp_GetUserClaims @email = '{email}'");

            if (dbResult.Count == 1)
            {
                string foundTelephoneNumberStr = Convert.ToString(dbResult[0].TelephoneNumber);
                string foundGlobalIdStr = Convert.ToString(dbResult[0].GlobalID);
                string foundOrganizationIdStr = Convert.ToString(dbResult[0].OrganizationIDs);

                _logger.Log(LogLevel.Information, $"Návratové hodnoty TelephoneNumber: '{foundTelephoneNumberStr}', GlobalId: '{foundGlobalIdStr}', OrganizationIds: '{foundOrganizationIdStr}'");

                return new OkObjectResult(new ResponseContent()
                {
                    extension_TelephoneNumber = foundTelephoneNumberStr,
                    extension_GlobalID = foundGlobalIdStr,
                    extension_OrganizationIDs = foundOrganizationIdStr,
                    extension_MAUser = true
                });

            }

            else
            {
                _logger.Log(LogLevel.Error, $"User '{email}' doesnt exist in database.");
                return new BadRequestObjectResult(new ResponseContent($"|API_ERROR_4|User doesnt exist in database|'{email}|'"));
            }

        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem();
        }

    }

    private static bool Authorize(HttpRequest req, ILogger _logger, bool isLocal, AuthorizationSettings _authsettings)
    {
        // get the environment's credentials 
        string passwordStored = string.Empty;
        string emailStored = string.Empty;

        emailStored = !isLocal ? _authsettings.SEmail : _authsettings.PEmail;
        passwordStored = isLocal ? _authsettings.SPassword : _authsettings.PPassword;


        // check if the HTTP Authorization header exist
        if (!req.Headers.ContainsKey("Authorization"))
        {
            _logger.Log(LogLevel.Error, "Missing HTTP basic authentication header.");
            return false;
        }

        // read the authorization header
        var auth = req.Headers["Authorization"].ToString();

        // ensure the type of the authorization header id `Basic`
        if (!auth.StartsWith("Basic "))
        {
            _logger.Log(LogLevel.Error, "HTTP basic authentication header must start with 'Basic '.");
            return false;
        }

        // get the the HTTP basic authorization credentials
        var cred = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
        string userNameCredentials = cred[0];
        string passwordCredentials = cred[1];

        // evaluate the credentials and return the result
        return (userNameCredentials == emailStored && passwordCredentials == passwordStored);
    }
}
