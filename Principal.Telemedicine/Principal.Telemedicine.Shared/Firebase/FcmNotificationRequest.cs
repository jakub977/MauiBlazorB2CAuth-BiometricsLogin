using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Firebase;

public class FcmNotificationRequest
{
    public int UserId { get; set; }

    public AppMessageContentTypeEnum AppMessageContentTypeEnum { get; set; }
}

