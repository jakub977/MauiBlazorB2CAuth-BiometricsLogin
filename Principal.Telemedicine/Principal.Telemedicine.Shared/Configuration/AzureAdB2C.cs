using System;
using System.Collections.Generic;
using System.Linq;
namespace Principal.Telemedicine.Shared.Configuration
{
    /// <summary>
    /// Konfigurační třída pro volání AD B2C
    /// </summary>
    public class AzureAdB2C
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool AllowWebApiToBeAuthorizedByACL { get; set; }
        public string CallbackPath { get; set; }
        public string SignedOutCallbackPath { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string B2CApplicationDomain { get; set; }
    }
}
