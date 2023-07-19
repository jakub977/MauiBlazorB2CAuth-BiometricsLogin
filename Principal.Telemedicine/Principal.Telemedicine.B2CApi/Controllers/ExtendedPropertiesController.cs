using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Principal.Telemedicine.B2CApi.Helpers;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Principal.Telemedicine.B2CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedPropertiesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ExtendedPropertiesController(ILogger logger)
        {
            _logger = logger;
        }

        // POST/ api/ExtendedPropertiesController> / {requestBody}
        [HttpPost("AddExtendedProperties", Name = "AddExtendedProperties")]
        public async Task<IActionResult> AddExtendedProperties(string requestBody)
        {
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (string.IsNullOrEmpty(requestBody))
            {
                // pokud přijde prázdné request body, logujeme
                if (data == null)
                {
                    _logger.LogInformation(requestBody);
                    return NotFound();
                }
            }

            _logger.LogInformation(requestBody);

            SqlConnection sqlConn = new SqlConnection("Server=tcp:tmworkstoresqlserver.database.windows.net,1433;Initial Catalog=VANDA_TEST;Persist Security Info=False;User ID=workstore_worker;Password=9hXo!dX#6rS2ccRhXRjvD%EiJV$qqbL5;Encrypt=True;Enlist=True;Pooling=True;Min Pool Size=1;Max Pool Size=300;ConnectRetryCount=3;TrustServerCertificate=False;Connection Timeout=450;");

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var b2cExtensionAppClientId = config.GetValue<string>("AzureAd:B2cExtensionAppClientId");
            if (string.IsNullOrWhiteSpace(b2cExtensionAppClientId))
            {
                throw new ArgumentException("B2cExtensionAppClientId (its Application ID) is missing from appsettings.json. Find it in the App registrations pane in the Azure portal. The app registration has the name 'b2c-extensions-app. Do not modify. Used by AADB2C for storing user data.'.", nameof(b2cExtensionAppClientId));
            }

            // namapování na custom claimy
            const string customPropertyName1 = "Telephone Number";
            const string customPropertyName2 = "Global ID";

            // získání názvu atributu včetně cesty (Azure AD extension)
            ExtendedPropertiesHelper helper = new Helpers.ExtendedPropertiesHelper(b2cExtensionAppClientId);
            string telephoneNumberClaimPath = helper.GetCompletePropertyName(customPropertyName1);
            string globalIdClaimPath = helper.GetCompletePropertyName(customPropertyName2);

            // response musí obsahovat verzi api a akci
            var extendedProperties = new Dictionary<string, string>();
            extendedProperties.Add("version", "1.0.0.");
            extendedProperties.Add("action", "Continue");

            // pokud jeden z claimů v requestu vůbec nepřišel, dohledáme jeho hodnotu v db
            if (data.telephoneNumberClaimPath == null || data.globalIdClaimPath == null)
            {
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
                                if (data.telephoneNumberClaimPath == null)
                                {
                                    extendedProperties.Add(telephoneNumberClaimPath, Convert.ToString(reader.GetValue(reader.GetOrdinal("TelephoneNumber"))));
                                }


                                if (data.globalIdClaimPath == null)
                                {
                                    extendedProperties.Add(globalIdClaimPath, Convert.ToString(reader.GetValue(reader.GetOrdinal("GlobalId"))));
                                }
                            }
                        }
                    }
                }

                return Ok(extendedProperties);
            }

            return Ok();
        }
    }
}
