namespace Principal.Telemedicine.Shared.Interfaces;

/// <summary>
/// Společné rozhraní pro operace pomocí Graph API
/// </summary>
public interface IGraphAPI
{
    /// <summary>
    /// Metoda vrátí tělo požadavku nutné pro odeslání emailu
    /// </summary>
    /// <param name="recipientsEmail">Email příjemce</param>
    /// <param name="messageSubject"> Předmět zprávy</param>
    /// <param name="messageBody">Tělo zprávy</param>
    /// <returns>Výsledek odeslání true / false</returns>
    string GetEmailRequestBody(string recipientsEmail, string messageSubject, string messageBody);

    /// <summary>
    /// Metoda vrátí access token uživatele v tenantu
    /// </summary>
    /// <param name="client">HTTP client</param>
    /// <param name="tenantId"> Identifikátor tenantu</param>
    /// <param name="requestContent">Obsah požadavku</param>
    /// <returns>Access token uživatele</returns>
    Task<string?> GetAccessTokenAsync(HttpClient client, string tenantId, FormUrlEncodedContent requestContent);

}
