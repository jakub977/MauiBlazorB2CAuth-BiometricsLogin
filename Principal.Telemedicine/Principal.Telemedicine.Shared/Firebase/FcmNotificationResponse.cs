using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Firebase;

public class FcmNotificationResponse
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
