﻿using Principal.Telemedicine.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models
{
    public class DiseaseOriginDetectionResultFromMLItemDataModel
    {
        public string Type { get; set; }
        public int UserId { get; set; }
        public DateTime DetectionDate { get; set; }
        public int DiseaseOriginTypeId { get; set; }
        public string DiseaseOriginTypeName { get; set; }
        public int ProbablityPercent { get; set; }
        public SymptomSeverityEnum Severity { get; set; }
        public int DiseaseOriginDetectionResultId { get; set; }
        //public bool IsActual => Type == "actual";
    }
}