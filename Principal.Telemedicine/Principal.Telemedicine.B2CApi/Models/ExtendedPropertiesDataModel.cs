namespace Principal.Telemedicine.B2CApi.Models;

/// <summary>
/// Data model definující vlastní přidané atributy/claimy nutné pro práci s uživatelem v rámci AD B2C.
/// </summary>
public class ExtendedPropertiesDataModel
{
    /// <summary>
    /// Jedinečný identifikátor uživatele.
    /// </summary>
    public string GlobalID { get; set; }

    /// <summary>
    /// Telefonní číslo vázané na daného uživatele.
    /// </summary>
    public string TelephoneNumber { get; set; }

    /// <summary>
    /// Identifikátor organizace, pod níž je uživatel založen.
    /// </summary>
    public int OrganizationIDs { get; set; }

    ///// <summary>
    ///// Příznak zda jde o uživatele mobilní aplikace.
    ///// </summary>
    //public bool? MAUser { get; set; }
}