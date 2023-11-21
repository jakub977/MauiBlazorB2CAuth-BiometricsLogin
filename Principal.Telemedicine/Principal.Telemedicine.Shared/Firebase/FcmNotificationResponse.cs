using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Firebase;

/// <summary>
/// Třída definující odpověď FCM na žádost o notifikaci
/// </summary>
public class FcmNotificationResponse
{
    /// <summary>
    /// Definuje, zdali byla notifikace úspěšně zpracována službou FCM
    /// </summary>
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Chybová zpráva
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; }

    /// <summary>
    /// Identifikátor notifikace, která byla úspěšně zpracována službou FCM
    /// </summary>
    public string MessageId { get; set; }
}
