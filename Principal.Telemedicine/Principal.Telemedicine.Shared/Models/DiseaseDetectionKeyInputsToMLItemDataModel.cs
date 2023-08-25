using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Models;

    /// <summary>
    /// Data model klíčových vstupů do ML, které jsou rozhodující pro určení diagnózy uživatele.
    /// </summary>
    public class DiseaseDetectionKeyInputsToMLItemDataModel
    {

        public int UserId { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public int? DiseaseSymptomTypeId { get; set; }
        public int? DiseaseSymptomCategoryId { get; set; }
        public int? PhysiologicalDataTypeId { get; set; }
        public int WeightPercent { get; set; }
        public decimal? Value { get; set; }
        public bool IsBooleanValue { get; set; }
        public string Range { get; set; }
        public SymptomSeverityEnum Severity { get; set; }
        public string UnitType { get; set; }
    }

