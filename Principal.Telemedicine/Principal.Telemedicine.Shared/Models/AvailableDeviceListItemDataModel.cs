
namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data model in/aktivních zařízení daného uživatele
/// </summary>
public class AvailableDeviceListItemDataModel
    {
        
        public string UserGlobalId { get; set; }
        public int IsConsent { get; set; }
        public string DeviceGlobalId { get; set; }
        public string DeviceProducerName { get; set; }
        public int? UserAccountConsentStatusTypeId { get; set; }
        public bool? IsOwnedByUser { get; set; }
        public bool? IsAbstract { get; set; }
        public int? DeviceCategoryId { get; set; }
        public bool? Active { get; set; }
    
    }

