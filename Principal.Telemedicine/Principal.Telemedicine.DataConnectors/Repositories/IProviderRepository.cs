using Microsoft.EntityFrameworkCore.Storage;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem Provider.
/// </summary>
public interface IProviderRepository
{
    /// <summary>
    /// Metoda vrací všechny poskytovatele.
    /// </summary>
    /// <param name="fullData">Příznak, zda vracet seznam včetně Efektivních uživatelů s vazbou na role a obrázky nebo jen data z tabulky Provider</param>
    /// <param name="organizationId">Id organizace</param>
    /// <returns>Seznam poskytovatelů</returns>
    Task<IEnumerable<Provider>> GetProvidersTaskAsync(bool fullData = true, int? organizationId = null);

    /// <summary>
    /// Metoda vrací konkrétního poskytovatele na základě id.
    /// </summary>
    /// <param name="id">ID poskytovatele</param>
    /// <returns>Konkrétní poskytovatel</returns>
    Task<Provider?> GetProviderByIdTaskAsync(int id);

    /// <summary>
    /// Metoda vrací detail poskytovatele pro list.
    /// </summary>
    /// <param name="id">ID poskytovatele</param>
    /// <returns>Konkrétní poskytovatel</returns>
    Task<Provider?> GetProviderListDetailByIdTaskAsync(int id);

    /// <summary>
    /// Metoda aktualizuje poskytovatele
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="provider">Poskytovatel</param>
    /// <returns>true / false</returns>
    Task<bool> UpdateProviderTaskAsync(CompleteUserContract currentUser, Provider provider);

    /// <summary>
    /// Metoda vytvoří poskytovatele
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="provider">Poskytovatel</param>
    /// <returns>true / false</returns>
    Task<bool> InsertProviderTaskAsync(CompleteUserContract currentUser, Provider provider);

    /// <summary>
    /// Metoda odstraní poskytovatele
    /// </summary>
    /// <param name="currentUser">Aktuální uživatel</param>
    /// <param name="provider">Poskytovatel</param>
    /// <returns>true / false</returns>
    Task<bool> DeleteProviderTaskAsync(CompleteUserContract currentUser, Provider provider);

    /// <summary>
    /// Metoda vrací všechny poskytovatele
    /// </summary>
    /// <returns>Query poskytovatelů</returns>
    public IQueryable<Provider> ListOfAllProviders();

    /// <summary>
    /// Metoda vrací konkrétního poskytovatele na základě id.
    /// </summary>
    /// <param name="id">ID poskytovatele</param>
    /// <returns>Konkrétní poskytovatel</returns>
    public Provider? GetProviderById(int providerId);
}

