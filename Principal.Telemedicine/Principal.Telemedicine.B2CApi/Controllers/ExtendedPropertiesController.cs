using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Principal.Telemedicine.B2CApi.Helpers;
using Principal.Telemedicine.B2CApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Principal.Telemedicine.B2CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedPropertiesController : ControllerBase
    {
        private readonly ILogger<ExtendedPropertiesController> _logger;

        public ExtendedPropertiesController(ILogger<ExtendedPropertiesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("AddExtendedProperties")]
        public async Task<IActionResult> AddExtendedProperties()
        {
            // Allowed domains
            //string[] allowedDomain = { "fabrikam.com", "fabricam.com" };

            // Check HTTP basic authorization
            //if (!Authorize(Request, _logger))
            //{
            //    _logger.LogWarning("HTTP basic authentication validation failed.");
            //    return (ActionResult)new UnauthorizedResult();
            //}

            var req = Request;

            // Get the request body

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogWarning("Request body is empty.");
                return (ActionResult)new OkObjectResult(new ResponseContent("ShowBlockPage", "Request body is empty."));
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // If input data is null, show block page
            if (data == null)
            {
                return (ActionResult)new OkObjectResult(new ResponseContent("ShowBlockPage", "There was a problem with your request."));
            }

            // Print out the request body
            _logger.LogInformation("Request body: " + requestBody);
            _logger.Log(LogLevel.Error, "Request body: " + requestBody);
            //_logger.Log(LogLevel.Error, "Request:" + request);

            // Get the current user language 
            string language = (data.ui_locales == null || data.ui_locales.ToString() == "") ? "default" : data.ui_locales.ToString();
            _logger.LogInformation($"Current language: {language}");

            // If email claim not found, show block page. Email is required and sent by default.
            if (data.email == null || data.email.ToString() == "" || data.email.ToString().Contains("@") == false)
            {
                return (ActionResult)new OkObjectResult(new ResponseContent("ShowBlockPage", "Email name is mandatory."));
            }


            //// If displayName claim doesn't exist, or it is too short, show validation error message. So, user can fix the input data.
            //if (data.displayName == null || data.displayName.ToString().Length < 5)
            //{
            //    return (ActionResult)new BadRequestObjectResult(new ResponseContent("ValidationError", "Please provide a Display Name with at least five characters."));
            //}

            // Input validation passed successfully, return `Allow` response.
            // TO DO: Configure the claims you want to return

            string telephoneNumberClaim = "extension_TelephoneNumber";
            string globalIdClaim = "extension_GlobalID";

            var extendedProperties = new Dictionary<string, string>();

            SqlConnection sqlConn = new SqlConnection();
         
            using (sqlConn)
            {

                using (SqlCommand cmd = new SqlCommand("dbo.sp_GetUserClaims", sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // vstupní parametr email
                    SqlParameter par = new SqlParameter("@email", SqlDbType.VarChar);
                    par.Value = data.email.ToString();
                    par.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(par);

                    sqlConn.Open();

                    // převzetí výsledku
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (data.telephoneNumberClaim == null)
                            {
                                extendedProperties.Add(telephoneNumberClaim, Convert.ToString(reader.GetValue(reader.GetOrdinal("TelephoneNumber"))));
                            }


                            if (data.globalIdClaim == null)
                            {
                                extendedProperties.Add(globalIdClaim, Convert.ToString(reader.GetValue(reader.GetOrdinal("GlobalId"))));
                            }
                        }
                    }
                }
            }
            if (extendedProperties.Count > 0)
            {
                return (ActionResult)new OkObjectResult(new ResponseContent(extendedProperties));
            }

            else
            {
                _logger.Log(LogLevel.Error, "Uživatel nebyl nalezen v databázi");
                return (ActionResult)new BadRequestObjectResult(new ResponseContent("ValidationError", "User was not found in database"));
            }

        }

        private static bool Authorize(HttpRequest req, ILogger log)
        {
            // Get the environment's credentials 
            //string username = System.Environment.GetEnvironmentVariable("BASIC_AUTH_USERNAME", EnvironmentVariableTarget.Process);
            //string password = System.Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD", EnvironmentVariableTarget.Process);

            string username = "TEST";
            string password = "HESLO";

            // Returns authorized if the username is empty or not exists.
            if (string.IsNullOrEmpty(username))
            {
                log.LogInformation("HTTP basic authentication is not set.");
                return true;
            }

            // Check if the HTTP Authorization header exist
            if (!req.Headers.ContainsKey("Authorization"))
            {
                log.LogWarning("Missing HTTP basic authentication header.");
                return false;
            }

            // Read the authorization header
            var auth = req.Headers["Authorization"].ToString();

            // Ensure the type of the authorization header id `Basic`
            if (!auth.StartsWith("Basic "))
            {
                log.LogWarning("HTTP basic authentication header must start with 'Basic '.");
                return false;
            }

            // Get the the HTTP basinc authorization credentials
            var cred = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');

            // Evaluate the credentials and return the result
            return (cred[0] == username && cred[1] == password);
        }
    }
}
