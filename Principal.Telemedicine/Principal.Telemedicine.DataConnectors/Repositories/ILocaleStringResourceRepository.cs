using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objektem LocaleStringResourceRepository.
/// </summary>
public interface ILocaleStringResourceRepository
{
    /// <summary>
    /// Metoda varcí objekt LocaleStringResource
    /// </summary>
    /// <param name="name">Název hodnoty</param>
    /// <param name="languageId">Id jazyka</param>
    /// <returns>LocaleStringResource</returns>
    Task<LocaleStringResource> GetLocaleStringResourceByNameAsync(string name, int languageId);

    /// <summary>
    /// Metoda vrací hodnotu z objektu LocaleStringResource
    /// </summary>
    /// <param name="name">Název hodnoty</param>
    /// <param name="languageId">Id jazyka</param>
    /// <returns>Hodnotu jazykového řetězce</returns>
    Task<string> GetLocaleStringResourceValueByNameAsync(string name, int languageId);
}

