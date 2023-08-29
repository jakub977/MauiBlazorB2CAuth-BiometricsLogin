using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Models;

    /// <summary>
    /// Data model predikce onemocnění.
    /// </summary>
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

