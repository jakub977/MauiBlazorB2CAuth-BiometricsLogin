using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Firebase;

/// <summary>
/// Pomocná třída pro komunikaci FCM - iOS
/// </summary>
public class Aps
{
    /// <summary>
    /// Atribut nastavení zvuku notifikace
    /// </summary>
    [JsonProperty("sound")]
    public string Sound { get; set; }

    /// <summary>
    /// Atribut kontentu notifikace
    /// </summary>
    [JsonProperty("content-available")]
    public int ContentAvailable { get; set; }
}
