using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Models;

    /// <summary>
    /// Data model výsledku predikce onemocnění s určitou pravděpodobností pro daného uživatele.
    /// </summary>
    public class DiseaseDetectionResultFromMLItemDataModel
    {
        
        public int UserId { get; set; }
        public string Type { get; set; }
        public DateTime DetectionDate { get; set; }
        public int DiseaseTypeId { get; set; }
        public string DiseaseTypeName { get; set; }
        public int? DiseaseOriginTypeId { get; set; }
        public string? DiseaseOriginTypeName { get; set; }
        public int ProbablityPercent { get; set; }
        public SymptomSeverityEnum Severity { get; set; }
        public int DiseaseDetectionResultId { get; set; }
    }

