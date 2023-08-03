using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models;

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

        //public bool? Active => ActiveInteger == null ? null : (bool?)(ActiveInteger.Value > 0);
        //public bool? Deleted => ActiveInteger == null ? null : (bool?)(ActiveInteger.Value < 0);
    }

