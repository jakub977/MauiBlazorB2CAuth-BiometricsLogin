using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem Provider.
/// </summary>
public interface IProviderRepository
{
    /// <summary>
    /// Metoda vrací všechny poskytovatele.
    /// </summary>
    /// <returns>Seznam poskytovatelů</returns>
    Task<IEnumerable<Provider>> GetProvidersTaskAsyncTask();

    /// <summary>
    /// Metoda vrací konkrétního poskytovatele na základě id.
    /// </summary>
    /// <param name="id">ID poskytovatele</param>
    /// <returns>Konkrétní poskytovatel</returns>
    Task<Provider?> GetProviderByIdTaskAsync(int id);

    /// <summary>
    /// Metoda aktualizuje poskytovatele
    /// </summary>
    /// <param name="provider">Poskytovatel</param>
    /// <returns>true / false</returns>
    Task<bool> UpdateProviderTaskAsync(Provider provider);
}

