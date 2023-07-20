using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Models
{
    public class PatientSubjectiveSymptomDataModel
    {
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        [JsonProperty("DiseaseSymptomCategoryId")]
        public int DiseaseSymptomCategoryId { get; set; }

        [JsonProperty("DiseaseSymptomCategory")]
        public string DiseaseSymptomCategory { get; set; }

        [JsonProperty("AggregateCreatedDateLocalTime")]
        public DateTime AggregateCreatedDateLocalTime { get; set; }

        [JsonProperty("TimeSlotStart")]
        public TimeSpan TimeSlotStart { get; set; }

        [JsonProperty("TimeSlotEnd")]
        public TimeSpan TimeSlotEnd { get; set; }

        [JsonProperty("DiseaseSymptomTypeId")]
        public int DiseaseSymptomTypeId { get; set; }

        [JsonProperty("DiseaseSymptomType")]
        public string DiseaseSymptomType { get; set; }

        [JsonProperty("DiseaseSymptomSubCategoryId")]
        public int DiseaseSymptomSubCategoryId { get; set; }

        [JsonProperty("DiseaseSymptomSubCategory")]
        public string DiseaseSymptomSubCategory { get; set; }

        [JsonProperty("Assessment")]
        public string Assessment { get; set; }

        public DiseaseSymptomCategoryEnum DiseaseSymptomCategoryEnum => (DiseaseSymptomCategoryEnum)System.Enum.ToObject(typeof(DiseaseSymptomCategoryEnum), DiseaseSymptomCategoryId);
    }
}
