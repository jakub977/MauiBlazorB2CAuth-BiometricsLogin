using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models
{
    public class PatientAggregatedSymptomProgressionDataModel
    {
        [JsonProperty("MeasuredValues")]
        public List<PatientMeasuredValueDataModel> MeasuredValues { get; set; } = new List<PatientMeasuredValueDataModel>();

        [JsonProperty("SubjectiveSymptoms")]
        public List<PatientSubjectiveSymptomDataModel> SubjectiveSymptoms { get; set; } = new List<PatientSubjectiveSymptomDataModel>();

        [JsonProperty("DiseasePrediction")]
        public List<PatientDiseasePredictionDataModel> DiseasePrediction { get; set; } = new List<PatientDiseasePredictionDataModel>();
    }
}
