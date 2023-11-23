using Principal.Telemedicine.DataConnectors.Models.Shared;

namespace Principal.Telemedicine.DataConnectors.Repositories;

/// <summary>
/// Pomocná třída základních operací nad objekty AppMessageAdditionalAttribute, AppMessageContentType, AppMessageContentType a AppMessageTemplate.
/// </summary>
public interface IAppMessageRepository
{
    /// <summary>
    /// Metoda vrátí náležitosti notifikace/zprávy podle Id typu notifikace/zprávy.
    /// </summary>
    /// <param name="contentTypeId"></param>
    /// <returns>Konkrétní AppMessageTemplate</returns>
    Task<AppMessageTemplate?> GetTemplateByContentTypeIdTaskAsync(int contentTypeId);

    /// <summary>
    /// Metoda vrátí dodatečné aktributy notifikace/zprávy podle Id typu notifikace/zprávy.
    /// </summary>
    /// <param name="contentTypeId"></param>
    /// <returns>Konkrétní AppMessageAdditionalAttribute</returns>
    Task<AppMessageAdditionalAttribute?> GetAdditionalAttributeByContentTypeIdTaskAsync(int contentTypeId);

    /// <summary>
    /// Metoda uloží záznam o odeslané notifikaci/zprávě službě FCM.
    /// </summary>
    /// <param name="sentLog"></param>
    /// <returns>bool value</returns>
    Task<bool> InsertSentLogTaskAsync(AppMessageSentLog sentLog);


}

