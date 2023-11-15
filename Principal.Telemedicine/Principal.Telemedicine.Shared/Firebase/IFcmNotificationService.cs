namespace Principal.Telemedicine.Shared.Firebase;

public interface IFcmNotificationService
{
    Task<string> SendFcmNotification(List<string> tokens, string title, string body);
}

