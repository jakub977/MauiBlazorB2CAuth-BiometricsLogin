using System.Text.Json.Serialization;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of current measured values. 
/// </summary>
public class UserMeasuredValuesDataModel
{
    /// <summary>
    /// Link to dbo.PhysiologicalDataType as a kind of measured physiological data
    /// </summary>
    [JsonPropertyName("PhysiologicalDataTypeId")]
    public int PhysiologicalDataTypeId { get; set; }

    /// <summary>
    /// Measured physiological data name
    /// </summary>
    [JsonPropertyName("PhysiologicalDataTypeName")]
    public string PhysiologicalDataTypeName { get; set; }

    /// <summary>
    /// Current measured value
    /// </summary>
    [JsonPropertyName("MeasuredValue")]
    public string MeasuredValue { get; set; }

    /// <summary>
    /// Link to dbo.PhysiologicalDataCreationType as a way the value was obtained
    /// </summary>
    [JsonPropertyName("PhysiologicalDataCreationTypeId")]
    public int PhysiologicalDataCreationTypeId { get; set; }
}
