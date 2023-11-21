using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.Shared.Utils;

namespace Principal.Telemedicine.Shared.Firebase;

/// <inheritdoc/>
public class FcmNotificationService : IFcmNotificationService
{
    
    private readonly FirebaseMessaging _messaging;
    private readonly FcmSettings _fcmSettings;

    public FcmNotificationService(IOptions<FcmSettings> fcmSettings)
    {
        _fcmSettings = fcmSettings.Value;

        var app = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(_fcmSettings.JsonServiceKey).CreateScoped(_fcmSettings.Scope),
            ProjectId = _fcmSettings.ApplicationIdentifier,
            ServiceAccountId = _fcmSettings.ServiceAccountId,

        });

        _messaging = FirebaseMessaging.GetMessaging(app);
    }

    /// <inheritdoc/>
    public async Task<FcmNotificationResponse> SendFcmNotification(string token, string? title, string? body, string? additionalAttribute, string? validToDate)
    {

        FcmNotificationResponse response = new FcmNotificationResponse();

        try
        {

            Message message = new Message();
            message.Token = token;
            message.Notification = new Notification();
            message.Notification.Title = title;
            message.Notification.Body = body;

            message.Android = new AndroidConfig();
            message.Android.Notification = new AndroidNotification();
            message.Android.Notification.Sound = "default";
            message.Android.TimeToLive = new TimeSpan(12, 0, 0);
            message.Android.Priority = Priority.High;

            message.Apns = new ApnsConfig();
            message.Apns.CustomData = new Dictionary<string, object>();
            Aps aps = new Aps();
            aps.Sound = "default";
            if (string.IsNullOrEmpty(body))
            {
                aps.ContentAvailable = 1;
            }
            else
            {
                aps.ContentAvailable = 0;
            }
            message.Apns.CustomData.Add("aps", aps);
            string apnsExpiration = DateTimeUtils.ToEpoch(DateTime.UtcNow.AddHours(12)).ToString();
            message.Apns.Headers = new Dictionary<string, string>
            {
                { "apns-expiration", apnsExpiration}
            };
            message.Data = new Dictionary<string, string>
            {
                { "AdditionalAttribute", additionalAttribute },
                { "ValidToDate", validToDate }
            };

            string fcmResponse = await _messaging.SendAsync(message);

            response.IsSuccess = true;
            response.MessageId = fcmResponse.Split('/').Last();

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;

            return response;
        }
    }
}
