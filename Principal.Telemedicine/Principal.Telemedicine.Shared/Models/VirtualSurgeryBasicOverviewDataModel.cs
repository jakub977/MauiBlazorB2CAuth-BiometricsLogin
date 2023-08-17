using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Models;

    public class VirtualSurgeryBasicOverviewDataModel
    {
        
        public int UserId { get; set; }
        public DateTime? IsolationStartDateUtc { get; set; }
        public DateTime? IsolationEndDateUtc { get; set; }
        public DateTime? IsolationNotificationDeliveryDateUtc { get; set; }
        public int? IsolationStateId { get; set; }
        public string? IsolationStateName { get; set; }
        public DateTime? InvitationToMedicalExaminationRequestDateUtc { get; set; }
        public DateTime? InvitationToMedicalExaminationDeliveryDateUtc { get; set; }
        public DateTime? DiseaseDetectionValidationCreatedDateUtc { get; set; }
        public int? DiseaseDetectionValidationTypeId { get; set; }
        public string? DiseaseDetectionValidationTypeName { get; set; }
        public int? DiseaseDetectionResultId { get; set; }
        public int? DiseaseOriginDetectionResultId { get; set; }
        public int? DiseaseTypeId { get; set; }
        public string? DiseaseTypeName { get; set; }
        public int? DiseaseOriginTypeId { get; set; }
        public string? DiseaseOriginTypeName { get; set; }
        public DateTime? QuarantineStartDateUtc { get; set; }
        public DateTime? QuarantineEndDateUtc { get; set; }
        public DateTime? QuarantineNotificationDeliveryDateUtc { get; set; }
        public int? QuarantineStateId { get; set; }
        public string? QuarantineStateName { get; set; }
    }

