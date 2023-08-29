using Newtonsoft.Json;

namespace Principal.Telemedicine.B2CApi.Models;

/// <summary>
/// Návratová třída definující odpověď AD B2C.
/// </summary>
public class ResponseContent
{
    public const string ApiVersion = "1.0.0";


    public ResponseContent()
    {
        this.version = ResponseContent.ApiVersion;
        this.action = "Continue";
    }

    /// <summary>
    /// Návratová třída pro vracení specifických chybových hlášek.
    /// </summary>
    /// <param name="errorMessage"> Specifická chybová hláška</param>
    public ResponseContent(string errorMessage)
    {
        this.version = ResponseContent.ApiVersion;
        this.action = "Continue";
        this.userMessage = errorMessage;
        if (!string.IsNullOrEmpty(errorMessage))
        {
            this.status = StatusCodes.Status400BadRequest.ToString();
        }
    }
    /// <summary>
    /// Verze API.
    /// </summary>
    public string version { get; }

    /// <summary>
    /// Požadovaná akce, zde vždy "Continue".
    /// </summary>
    public string action { get; set; }

    /// <summary>
    /// Specifická chybová hláška v odpovědi.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string userMessage { get; set; }

    /// <summary>
    /// Stavový kód odpovědi.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string status { get; set; }

    /// <summary>
    /// Vlastní přidaný atribut odpovídající telefonnímu číslu uživatele.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string extension_TelephoneNumber { get; set; }

    /// <summary>
    /// Vlastní přidaný atribut odpovídající globálnímu identifikátoru uživatele.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string extension_GlobalID { get; set; }

    /// <summary>
    /// Vlastní přidaný atribut odpovídající identifikátoru organizace, pod níž je uživatel založen. 
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string extension_OrganizationIDs { get; set; }

}

