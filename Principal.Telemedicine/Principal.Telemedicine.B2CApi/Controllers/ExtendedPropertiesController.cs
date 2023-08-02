using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Principal.Telemedicine.B2CApi.Helpers;
using Principal.Telemedicine.B2CApi.Models;
using Principal.Telemedicine.Shared.Models;
using Principal.Telemedicine.SharedApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Principal.Telemedicine.B2CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedPropertiesController : ControllerBase
    {
        private readonly ILogger<ExtendedPropertiesController> _logger;
        private readonly ApiDbContext _context;

        public ExtendedPropertiesController(ILogger<ExtendedPropertiesController> logger, ApiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("AddExtendedProperties")]
        public async Task<IActionResult> AddExtendedProperties()
        {
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
                return new OkObjectResult(new ResponseContent("Request body is empty."));
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // If input data is null, show block page
            if (data == null)
            {
                return new OkObjectResult(new ResponseContent("There was a problem with your request."));
            }

            _logger.Log(LogLevel.Error, "Request:" + requestBody);

            string email = Convert.ToString(data.Email);

            List<ExtendedPropertiesDataModel> dbResult = new List<ExtendedPropertiesDataModel>();

            _logger.Log(LogLevel.Error, $"Volání db s parametrem email: '{email}' ");
            dbResult = _context.ExecSqlQuery<ExtendedPropertiesDataModel>($"dbo.sp_GetUserClaims @email = '{email}'");

            if (dbResult.Count == 1)
            {
                string foundTelephoneNumberStr = Convert.ToString(dbResult[0].TelephoneNumber);
                string foundGlobalIdStr = Convert.ToString(dbResult[0].GlobalID);

                _logger.Log(LogLevel.Error, $"Návratové hodnoty TelephoneNumber: '{foundTelephoneNumberStr}', GlobalId: '{foundGlobalIdStr}'");

                return new OkObjectResult(new ResponseContent()
                {
                    extension_TelephoneNumber = foundTelephoneNumberStr,
                    extension_GlobalID = foundGlobalIdStr,
                });

            }

            else
            {
                return new BadRequestObjectResult(new ResponseContent("User doesnt exist in database."));
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
