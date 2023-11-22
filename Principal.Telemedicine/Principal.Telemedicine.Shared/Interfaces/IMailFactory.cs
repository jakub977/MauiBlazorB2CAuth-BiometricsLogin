namespace Principal.Telemedicine.Shared.Interfaces;
/// <summary>
/// Společné rozhraní pro operace s mailem
/// </summary>
public interface IMailFactory
{
    /// <summary>
    /// Odešle email
    /// </summary>
    /// <param name="recipientsEmail">Email příjemce</param>
    /// <param name="messageBody">Tělo zprávy</param>
    /// <param name="messageSubject"> Předmět zprávy</param>
    /// <returns>Vrací <see langword="true"/> v případě úspěšného odeslání emailu, <see langword="false"/> pokud email nebyl odeslán.</returns>        
    Task<bool> SendEmailAsyncTask(string recipientsEmail, string messageBody, string messageSubject);
}
