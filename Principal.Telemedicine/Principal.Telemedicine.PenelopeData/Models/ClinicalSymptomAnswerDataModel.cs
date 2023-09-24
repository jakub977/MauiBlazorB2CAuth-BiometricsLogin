namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of answers to clinical symptom questions.
/// </summary>
public class ClinicalSymptomAnswerDataModel
{
    /// <summary>
    /// Primary identifier of an assessment
    /// </summary>
    public int DiseaseSymptomSubjectiveAssessmentToSelectionId { get; set; }

    /// <summary>
    /// User's global identifier
    /// </summary>
    public string GlobalId { get; set; }

    /// <summary>
    /// Numerical value of a numeric scale question
    /// </summary>
    public int? NumericalValue { get; set; }

    /// <summary>
    /// Date and time the question was answered
    /// </summary>
    public DateTime? AnsweredDateUtc { get; set; }
}

