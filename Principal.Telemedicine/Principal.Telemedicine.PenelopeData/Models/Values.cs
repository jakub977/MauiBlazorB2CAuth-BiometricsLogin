using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Helper class of physiological data values
/// </summary>
public class Values
{
    /// <summary>
    /// Measured physiological data name
    /// </summary>
    [JsonProperty("MeasuredValue")]
    public string MeasuredValue { get; set; }

    /// <summary>
    /// Date and time the data was taken
    /// </summary>
    [JsonProperty("MeasuredTime")]
    public DateTime MeasuredTime { get; set; }

    /// <summary>
    /// Link to dbo.PhysiologicalDataCreationType as a way the value was obtained
    /// </summary>
    [JsonProperty("PhysiologicalDataCreationTypeId")]
    public int? PhysiologicalDataCreationTypeId { get; set; }
}

