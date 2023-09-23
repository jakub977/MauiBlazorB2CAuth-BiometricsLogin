namespace Principal.Telemedicine.PenelopeData.Models;

public class ClinicalSymptomAnswerDataModel
{
    public int DiseaseSymptomSubjectiveAssessmentToSelectionId { get; set; }
    public string GlobalId { get; set; }
    public int? NumericalValue { get; set; }
    public DateTime? AnsweredDateUtc { get; set; }
}

