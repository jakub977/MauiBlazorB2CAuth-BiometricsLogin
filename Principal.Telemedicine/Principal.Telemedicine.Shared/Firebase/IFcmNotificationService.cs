namespace Principal.Telemedicine.Shared.Firebase;

public interface IFcmNotificationService
{
    Task<FcmNotificationResponse> SendFcmNotification(string token, string title, string body);
}

