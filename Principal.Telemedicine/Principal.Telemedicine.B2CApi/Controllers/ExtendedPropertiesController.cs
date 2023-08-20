using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Principal.Telemedicine.B2CApi.Models;
using System.Text;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.DataConnectors.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Principal.Telemedicine.B2CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedPropertiesController : ControllerBase
    {
        private readonly ILogger<ExtendedPropertiesController> _logger;
        private readonly ApiDbContext _context;
        private readonly AuthorizationSettings _authsettings;

        public ExtendedPropertiesController(ILogger<ExtendedPropertiesController> logger, ApiDbContext context, IOptions<AuthorizationSettings> authsettings)
        {
            _logger = logger;
            _context = context;
            _authsettings = authsettings?.Value;
        }

        [HttpPost]
        [Route("AddExtendedProperties")]
        public async Task<IActionResult> AddExtendedProperties()
        {
            string environmentName = Environment.GetEnvironmentVariable(variable: "ASPNETCORE_ENVIRONMENT").ToLowerInvariant();
            var req = Request;

            // Check HTTP basic authorization
            if (!Authorize(req, _logger, environmentName, _authsettings))
            {
                _logger.Log(LogLevel.Error, "HTTP basic authentication validation failed.");
                return new UnauthorizedResult();
            }

            // Get the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.Log(LogLevel.Error, $"Request body: '{requestBody}' ");

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.Log(LogLevel.Error, "Request body is empty.");
                return new BadRequestObjectResult(new ResponseContent("Request body is empty."));
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data == null)
            {
                _logger.Log(LogLevel.Error, "Deserialization of request body was not successfull.");
                return new BadRequestObjectResult(new ResponseContent("Deserialization of request body was not successfull."));
            }

            if (data.email == null)
            {
                _logger.Log(LogLevel.Error, "Email is mandatory and is empty.");
                return new BadRequestObjectResult(new ResponseContent("Email is mandatory and is empty"));
            }

            string email = Convert.ToString(data.email);

            List<ExtendedPropertiesDataModel> dbResult = new List<ExtendedPropertiesDataModel>();

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
                    extension_OrganizationIDs = foundOrganizationIdStr
                });

            }

            else
            {
                _logger.Log(LogLevel.Error, "User doesnt exist in database.");
                return new BadRequestObjectResult(new ResponseContent("User doesnt exist in database."));
            }

        }

        private static bool Authorize(HttpRequest req, ILogger _logger, string environmentName, AuthorizationSettings _authsettings)
        {
            //Get the environment's credentials 
            string passwordStored = string.Empty;
            string emailStored = string.Empty;

            emailStored = !string.IsNullOrEmpty(environmentName) && environmentName != "local" ? _authsettings.SEmail : _authsettings.PEmail;
            passwordStored = !string.IsNullOrEmpty(environmentName) && environmentName != "local" ? _authsettings.SPassword : _authsettings.PPassword;


            // Check if the HTTP Authorization header exist
            if (!req.Headers.ContainsKey("Authorization"))
            {
                _logger.Log(LogLevel.Error, "Missing HTTP basic authentication header.");
                return false;
            }

            // Read the authorization header
            var auth = req.Headers["Authorization"].ToString();

            // Ensure the type of the authorization header id `Basic`
            if (!auth.StartsWith("Basic "))
            {
                _logger.Log(LogLevel.Error, "HTTP basic authentication header must start with 'Basic '.");
                return false;
            }

            // Get the the HTTP basinc authorization credentials
            var cred = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
            string userNameCredentials = cred[0];
            string passwordCredentials = cred[1];

            // Evaluate the credentials and return the result
            return (userNameCredentials == emailStored && passwordCredentials == passwordStored);
        }
    }
}
