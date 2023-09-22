namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of questions about clinical symptoms.
/// </summary>
public class ClinicalSymptomQuestionDataModel
{
    /// <summary>
    /// Primary identifier of an assessment
    /// </summary>
    public int DiseaseSymptomSubjectiveAssessmentToSelectionId { get; set; }

    /// <summary>
    /// Primary identifier of an assessment type
    /// </summary>
    public int DiseaseSymptomSubjectiveAssessmentTypeId { get; set; }

    /// <summary>
    /// Assessment type name
    /// </summary>
    public string DiseaseSymptomSubjectiveAssessmentType { get; set; }

    /// <summary>
    /// Disease symptom type name selected by patient
    /// </summary>
    public string SelectedDiseaseSymptomType { get; set; }

    /// <summary>
    /// Date of last update, using coordinated universal time
    /// </summary>
    public DateTime LastUpdateDateUtc { get; set; }
}
