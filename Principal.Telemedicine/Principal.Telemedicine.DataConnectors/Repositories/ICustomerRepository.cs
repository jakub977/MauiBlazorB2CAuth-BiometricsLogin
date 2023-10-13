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
}

