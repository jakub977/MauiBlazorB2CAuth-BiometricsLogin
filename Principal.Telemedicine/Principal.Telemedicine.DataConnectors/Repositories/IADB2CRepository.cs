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
    /// <returns>Výsledek uložení true / false</returns>
    Task<bool> UpdateUserAsyncTask(Customer customer);

    /// <summary>
    /// Metoda založí nového uživatele 
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Výsledek uložení true / false</returns>
    Task<bool> InsertUserAsyncTask(Customer customer);

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

