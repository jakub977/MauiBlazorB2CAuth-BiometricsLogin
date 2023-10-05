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
    /// <param name="user">Customer</param>
    /// <returns>true / false</returns>
    Task<bool> UpdateCustomerTaskAsync(Customer user);
}

