using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.B2CApi
{
    public class AuthorizationSettings
    {
        public string PEmail { get; set; }

        [SecretValue]
        public string SEmail { get; set; }

        public string PPassword { get; set; }

        [SecretValue]
        public string SPassword { get; set; }


    }
}
