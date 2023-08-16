using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Principal.Telemedicine.Shared.Enums;

namespace Principal.Telemedicine.Shared.Models
{
    public class DiseaseDetectionResultFromMLItemDataModel
    {
        [Key]
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

        //public bool IsActual => Type == "actual";
    }
}
