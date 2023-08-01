using Newtonsoft.Json;

namespace Principal.Telemedicine.B2CApi.Models
{
    public class ResponseContent
    {
        public const string ApiVersion = "1.0.0";
        private Dictionary<string, string> extendedProperties;

        public ResponseContent(Dictionary<string, string> extendedProperties)
        {
            this.version = ResponseContent.ApiVersion;
            this.action = "Continue";
            this.extendedProperties = extendedProperties;
        }


        public ResponseContent(string action, string userMessage)
        {
            this.version = ResponseContent.ApiVersion;
            this.action = action;
            this.userMessage = userMessage;
            if (action == "ValidationError")
            {
                this.status = "400";
            }
        }

        public string version { get; }
        public string action { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string userMessage { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string status { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extension_TelephoneNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extension_GlobalID { get; set; }
    }
}

