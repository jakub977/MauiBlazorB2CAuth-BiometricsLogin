using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models
{
    public class MedicalDeviceMeasuringHistoryItemDataModel
    {
        public int DeviceId { get; set; }
        public int DeviceProducerId { get; set; }
        public string DeviceProducerName { get; set; }
        public int DeviceTypeId { get; set; }
        public string DeviceTypeName { get; set; }
        public int UserId { get; set; }
        public DateTime MeasuredDateUtc { get; set; }
        public string MeasuredValue { get; set; }
        public int PhysiologicalDataCreationTypeId { get; set; }
        public int PhysiologicalDataTypeId { get; set; }
        public string PhysiologicalDataTypeName { get; set; }
        public string UnitType { get; set; }
    }
}
