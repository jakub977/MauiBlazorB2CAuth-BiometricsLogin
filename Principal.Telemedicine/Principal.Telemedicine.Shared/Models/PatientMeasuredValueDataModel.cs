using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Models
{
    public class PatientMeasuredValueDataModel
    {
        private decimal _measuredValueDecimalMin;
        private decimal _measuredValueDecimalMax;

        [JsonProperty("DataType")]
        public string DataType { get; set; }

        [JsonProperty("MeasuredValueType")]
        public string MeasuredValueType { get; set; }

        [JsonProperty("MeasuredValueTypeId")]
        public int MeasuredValueTypeId { get; set; }

        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("MeasuredValueIntMin")]
        public int MeasuredValueIntMin { get; set; }

        [JsonProperty("MeasuredValueIntMax")]
        public int MeasuredValueIntMax { get; set; }

        [JsonProperty("MeasuredValueDecimalMin")]
        public decimal MeasuredValueDecimalMin
        {
            get => PhysiologicalDataType == PhysiologicalDataTypeEnum.Brpm
                   || PhysiologicalDataType == PhysiologicalDataTypeEnum.Glycemia
                   || PhysiologicalDataType == PhysiologicalDataTypeEnum.Temperature
                ? _measuredValueDecimalMin
                : MeasuredValueIntMin;

            set => _measuredValueDecimalMin = value;
        }

        [JsonProperty("MeasuredValueDecimalMax")]
        public decimal MeasuredValueDecimalMax
        {
            get => PhysiologicalDataType == PhysiologicalDataTypeEnum.Brpm
                   || PhysiologicalDataType == PhysiologicalDataTypeEnum.Glycemia
                   || PhysiologicalDataType == PhysiologicalDataTypeEnum.Temperature
                ? _measuredValueDecimalMax
                : MeasuredValueIntMax;

            set => _measuredValueDecimalMax = value;
        }

        [JsonProperty("MeasuredValueText")]
        public string MeasuredValueText { get; set; }


        [JsonProperty("AggregateMeasuredDateLocalTime")]
        public DateTime AggregateMeasuredDateLocalTime { get; set; }

        [JsonProperty("TimeSlotStart")]
        public TimeSpan TimeSlotStart { get; set; }

        [JsonProperty("TimeSlotEnd")]
        public TimeSpan TimeSlotEnd { get; set; }

        public PhysiologicalDataTypeEnum PhysiologicalDataType =>
            (PhysiologicalDataTypeEnum)System.Enum.ToObject(typeof(PhysiologicalDataTypeEnum), MeasuredValueTypeId);
    }
}
