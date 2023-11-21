namespace Principal.Telemedicine.Shared.Firebase;

/// <summary>
/// Rozhraní pro komunikaci s FCM
/// </summary>
public interface IFcmNotificationService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="token">FCM token uživatele mobilní aplikace</param>
    /// <param name="title">Title notifikace/zprávy</param>
    /// <param name="body">Obsah notifikace/zprávy</param>
    /// <param name="additionalAttribute">Dodatečný atribut notifikace/zprávy</param>
    /// <param name="validToDate">Platnost notifikace/zprávy</param>
    /// <returns>Konkrétní FcmNotificationResponse</returns>
    Task<FcmNotificationResponse> SendFcmNotification(string token, string? title, string? body, string? additionalAttribute, string? validToDate);
}

