using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models
{
    public class PatientDiseasePredictionDataModel
    {
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        [JsonProperty("CreatedDateLocalTime")]
        public DateTime CreatedDateLocalTime { get; set; }

        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("DiseaseTypeId")]
        public int DiseaseTypeId { get; set; }

        [JsonProperty("DiseaseType")]
        public string DiseaseType { get; set; }
    }
}
