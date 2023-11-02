using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Firebase;

public class Aps
{
    [JsonProperty("sound")]
    public string Sound { get; set; }

    [JsonProperty("content-available")]
    public int ContentAvailable { get; set; }
}
