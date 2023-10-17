using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem Customer.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Metoda vrací všechny uživatele.
    /// </summary>
    /// <returns> Seznam uživatelů </returns>
    Task<IEnumerable<Customer>> GetCustomersTaskAsyncTask();

    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Konkrétní uživatel </returns>
    Task<Customer?> GetCustomerByIdTaskAsync(int id);

    /// <summary>
    /// Metoda vrací konkrétního uživatele na základě id.
    /// </summary>
    /// <param name="globalId"></param>
    /// <returns> Konkrétní uživatel </returns>
    Task<Customer?> GetCustomerByGlobalIdTaskAsync(string globalId);

    /// <summary>
    /// Metoda aktualizuje Customera včetně aktualizace v ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <param name="ignoreADB2C"></param>
    /// <returns>true / false</returns>
    Task<bool> UpdateCustomerTaskAsync(Customer currentUser, Customer user, bool? ignoreADB2C = false);

    /// <summary>
    /// Metoda založí nového Customera včetně založení v ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <returns>true / false</returns>
    Task<bool> InsertCustomerTaskAsync(Customer currentUser, Customer user);

    /// <summary>
    /// Označí užvatele za smazaného a smaže ho z ADB2C
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Customer</param>
    /// <param name="ignoreADB2C">Nemezat v ADB2C?</param>
    /// <returns>true / false</returns>
    Task<bool> DeleteCustomerTaskAsync(Customer currentUser, Customer user, bool? ignoreADB2C = false);

    /// <summary>
    /// Zkontroluje, zda uživatel (nesmazaný) již existuje v dedikované DB podel Emailu, tel. čísla 1 a tel. čísla 2, GlobalId nebo PersonalIdentificationNumber
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="user">Uživatel ke kontrole</param>
    /// <returns>0 jako že se shodný uživatel nenalezl nebo:
    /// -10 = uživatel se stejným emailem existuje
    /// -11 = uživatel se stejným tel. číslem existuje
    /// -12 = uživatel se stejným PersonalIdentificationNumber existuje
    /// -13 = uživatel se stejným GlobalID existuje
    /// </returns>
    Task<int> CheckIfUserExists(Customer currentUser, Customer user);
}

