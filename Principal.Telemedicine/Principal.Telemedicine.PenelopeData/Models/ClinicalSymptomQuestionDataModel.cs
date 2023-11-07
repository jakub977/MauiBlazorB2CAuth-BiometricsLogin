using System.Text.Json.Serialization;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of questions about clinical symptoms.
/// </summary>
public class ClinicalSymptomQuestionDataModel
{
    /// <summary>
    /// Primary identifier of an assessment
    /// </summary>
    [JsonPropertyName("DiseaseSymptomSubjectiveAssessmentToSelectionId")]
    public int DiseaseSymptomSubjectiveAssessmentToSelectionId { get; set; }

    /// <summary>
    /// Primary identifier of an assessment type
    /// </summary>
    [JsonPropertyName("DiseaseSymptomSubjectiveAssessmentTypeId")]
    public int DiseaseSymptomSubjectiveAssessmentTypeId { get; set; }

    /// <summary>
    /// Assessment type name
    /// </summary>
    [JsonPropertyName("DiseaseSymptomSubjectiveAssessmentType")]
    public string DiseaseSymptomSubjectiveAssessmentType { get; set; }

    /// <summary>
    /// Disease symptom type name selected by patient
    /// </summary>
    [JsonPropertyName("SelectedDiseaseSymptomType")]
    public string SelectedDiseaseSymptomType { get; set; }

    /// <summary>
    /// Date of last update, using coordinated universal time
    /// </summary>
    [JsonPropertyName("LastUpdateDateUtc")]
    public DateTime LastUpdateDateUtc { get; set; }
}
