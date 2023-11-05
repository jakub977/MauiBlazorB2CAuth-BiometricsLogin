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
        public string? Instance { get; set; }
        public string? Domain { get; set; }

        /// <summary>
        /// Veřejný identifikátor rozšíření klienta. 
        /// </summary>
        public string? B2cExtensionAppClientId { get; set; }

        /// <summary>
        /// Veřejný identifikátor tenantu. 
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Veřejný identifikátor klienta. 
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Veřejný secret klienta. 
        /// </summary>
        public string? ClientSecret { get; set; }
        public bool AllowWebApiToBeAuthorizedByACL { get; set; }
        public string? CallbackPath { get; set; }
        public string? SignedOutCallbackPath { get; set; }
        public string? SignUpSignInPolicyId { get; set; }

        /// <summary>
        /// Skrytý identifikátor rozšíření klienta  
        /// </summary>
        [SecretValue]
        public string? SB2cExtensionAppClientId { get; set; }

        /// <summary>
        /// Veřejná aplikační doména. 
        /// </summary>
        public string? B2CApplicationDomain { get; set; }

        /// <summary>
        /// Skrytý identifikátor tenantu. 
        /// </summary>
        [SecretValue]
        public string? STenantId { get; set; }

        /// <summary>
        /// Skrytý identifikátor klienta. 
        /// </summary>
        [SecretValue]
        public string? SClientId { get; set; }

        /// <summary>
        /// Skrytý secret klienta. 
        /// </summary>
        [SecretValue]
        public string? SClientSecret { get; set; }

        /// <summary>
        /// Skrytá aplikační doména. 
        /// </summary>
        [SecretValue]
        public string? SB2CApplicationDomain { get; set; }
    }
}
