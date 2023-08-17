namespace Principal.Telemedicine.B2CApi.Helpers
{
    internal class ExtendedPropertiesHelper
    {
        internal readonly string _b2cExtensionAppClientId;

        internal ExtendedPropertiesHelper(string b2cExtensionAppClientId)
        {
            _b2cExtensionAppClientId = b2cExtensionAppClientId.Replace("-", "");
        }

        internal string GetCompletePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new System.ArgumentException("Parameter cannot be null", nameof(propertyName));
            }

            return $"extension_{_b2cExtensionAppClientId}_{propertyName}";
        }
    }
}
