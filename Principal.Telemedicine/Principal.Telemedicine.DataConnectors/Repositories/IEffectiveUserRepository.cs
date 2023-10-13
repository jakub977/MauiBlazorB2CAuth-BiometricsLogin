using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem EffectiveUser.
/// </summary>
public interface IEffectiveUserRepository
{
    /// <summary>
    /// Metoda vrací všechny EffectiveUser.
    /// </summary>
    /// <returns>Seznam EffectiveUser</returns>
    Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsyncTask();

    /// <summary>
    /// Vrátí seznam Efektivních uživatelů danného uživatele
    /// </summary>
    /// <param name="userId">ID uživatele</param>
    /// <returns>Seznam EffectiveUser</returns>
    Task<IEnumerable<EffectiveUser>> GetEffectiveUsersTaskAsyncTask(int userId);

    /// <summary>
    /// Metoda vrací konkrétního EffectiveUser na základě id.
    /// </summary>
    /// <param name="id">ID EffectiveUser</param>
    /// <returns>Konkrétní EffectiveUser</returns>
    Task<EffectiveUser?> GetEffectiveUserByIdTaskAsync(int id);

    /// <summary>
    /// Metoda aktualizuje EffectiveUser
    /// </summary>
    /// <param name="user">EffectiveUser</param>
    /// <returns>true / false</returns>
    Task<bool> UpdateEffectiveUserTaskAsync(EffectiveUser user);

    /// <summary>
    /// Metoda zakládá nového EffectiveUser
    /// </summary>
    /// <param name="user">EffectiveUser</param>
    /// <returns>true / false</returns>
    Task<bool> InsertEffectiveUserTaskAsync(EffectiveUser user);

    /// <summary>
    /// Metoda vrací seznam nesmazaných Efektivních uživatelů daného poskytovatele.
    /// </summary>
    /// <returns>Seznam EffectiveUser</returns>
    Task<IEnumerable<EffectiveUser>> GetEffectiveUsersByProviderIdTaskAsync(int providerId);

    /// <summary>
    /// Metoda vrací nesmazané Efektivní uživatele daného poskytovatele.
    /// </summary>
    /// <returns>Seznam EffectiveUser</returns>
    IQueryable<EffectiveUser> GetEffectiveUsersByProviderId(int providerId);

    /// <summary>
    /// Metoda vrací nesmazané Efektivní uživatele dané organizace.
    /// </summary>
    /// <returns>Seznam EffectiveUser</returns>
    IQueryable<EffectiveUser> GetEffectiveUsersByOrganizationId(int organizationId);
}

