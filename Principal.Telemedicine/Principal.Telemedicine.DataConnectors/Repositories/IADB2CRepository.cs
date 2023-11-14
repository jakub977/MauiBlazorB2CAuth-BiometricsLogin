using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad AD B2C.
/// </summary>
public interface IADB2CRepository
{
    /// <summary>
    /// Metoda aktualizuje existujícího uživatele 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>1 - update se povedl nebo:
    /// -1 = globální chyba
    /// -15 = uživatel je nový, nemá UserId > 0
    /// -16 = uživatel nenalezen v AD B2C
    /// -17 = existuje více uživatelů v AD B2C</returns>
    Task<int> UpdateUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda založí nového uživatele 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>1 - založení se se povedlo nebo:
    /// -1 = globální chyba
    /// -18 = uživatel již existuje v AD B2C
    /// -19 = uživatel se stejným emailem již existuje v AD B2C</returns>
    Task<int> InsertUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda smaže uživatele
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek smazání true / false</returns>
    Task<bool> DeleteUserAsyncTask(Customer customer);


    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě objectId.
    /// </summary>
    /// <param name="id">ID objektu</param>
    /// <returns>Konkrétní uživatel</returns>
    Task<Customer?> GetUserByObjectIdAsyncTask(string objectId);

    /// <summary>
    /// Metoda odesílá mail uživateli
    /// <param name="recipientsEmail">email příjemce</param>
    /// <param name="messageBody">tělo zprávy</param>
    /// <param name="messageTitle">nadpis zprávy</param>
    /// <returns>Výsledek odeslání true / false</returns>
    Task<bool> SendEmailAsyncTask(string recipientsEmail, string messageBody, string messageTitle);

    /// <summary>
    /// Metoda kontroluje, zda je uživatel založen v ADB2C
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>1 - existuje, 0 - neexistuje, -1 - chyba</returns>
    Task<int> CheckUserAsyncTask(Customer customer);

    /// <summary>
    /// Vytvoří UPN z emailu. UPN je tvořeno emailem kódovaným jako Base64 a přidáním "@aplikační doména".
    /// V Base64 řetězci je pak nahrazen znak "=" znakem "_", jinak by nešlo UPN uložit v ADB2C
    /// </summary>
    /// <param name="email">email</param>
    /// <returns>UPN</returns>
    string CreateUPN(string email);

    /// <summary>
    /// Pomocná funkce pro kontrolu GlobalId, zda má nový formán (UPN  z AD B2C)
    /// </summary>
    /// <returns>Aplikační doménu</returns>
    string? GetApplicationDomain();

}

