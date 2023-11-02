namespace Principal.Telemedicine.Shared.Enums;

/// <summary>
/// Seznam předdefinovaných rolí
/// </summary>
public enum RoleEnum
{
    /// <summary>
    /// Super administrátor
    /// </summary>
    SuperAdmin = 1,
    /// <summary>
    /// Správce organizace
    /// </summary>
    OrganizationAdmin = 2,
    /// <summary>
    /// Správce poskytovatele
    /// </summary>
    ProviderAdmin = 3,
    /// <summary>
    /// Dohled
    /// </summary>
    SupervisoryPerson = 4,
    /// <summary>
    /// Lékař
    /// </summary>
    Doctor = 5,
    /// <summary>
    /// Pacient
    /// </summary>
    Patient = 6,
    /// <summary>
    /// Výzkum
    /// </summary>
    Research = 7
}
