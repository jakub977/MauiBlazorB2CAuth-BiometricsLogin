using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Firebase;

/// <summary>
/// Třída definující požadavek na notifikaci
/// </summary>
public class FcmNotificationRequest
{
    /// <summary>
    /// Určuje, zdali mají být notifikováni všichni uživatelé nebo konkrétní uživatel
    /// </summary>
    public bool NotifyAllUsers { get; set; }

    /// <summary>
    /// Identifikátor notifikovaného uživatele
    /// </summary>
    public string? UserGlobalId { get; set; }

    /// <summary>
    /// Číselník typů notifikací/zpráv
    /// </summary>
    public AppMessageContentTypeEnum AppMessageContentTypeEnum { get; set; }

    /// <summary>
    /// Nepovinný parametr určující platnost notifikace
    /// </summary>
    public string? ValidToDate { get; set; }
}

