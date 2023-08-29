using Newtonsoft.Json;

namespace Principal.Telemedicine.Shared.Models;

    /// <summary>
    /// Data model agregovaných naměřených hodnot, subjektivních příznak a predikcí.
    /// </summary>
    public class PatientAggregatedSymptomProgressionDataModel
    {
        [JsonProperty("MeasuredValues")]
        public List<PatientMeasuredValueDataModel> MeasuredValues { get; set; } = new List<PatientMeasuredValueDataModel>();

        [JsonProperty("SubjectiveSymptoms")]
        public List<PatientSubjectiveSymptomDataModel> SubjectiveSymptoms { get; set; } = new List<PatientSubjectiveSymptomDataModel>();

        [JsonProperty("DiseasePrediction")]
        public List<PatientDiseasePredictionDataModel> DiseasePrediction { get; set; } = new List<PatientDiseasePredictionDataModel>();
    }

