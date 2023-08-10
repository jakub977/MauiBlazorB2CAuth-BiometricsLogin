namespace Principal.Telemedicine.Shared.Configuration;
/// <summary>
/// Označí hodnotu konfigurace jako tajemství.
/// </summary>
/// <remarks>
/// Je nutné nastavit u všech vlastností, u který se předpokládá ukládání zabezpečených hodnot (připojovací řetězce, api klíče apod.).
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class SecretValueAttribute : Attribute
{
}
