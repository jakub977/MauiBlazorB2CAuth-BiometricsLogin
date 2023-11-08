namespace Principal.Telemedicine.Shared.Firebase;

public interface IFcmNotificationService
{
    Task<string> SendFcmNotification(string token, string title, string body);
}

