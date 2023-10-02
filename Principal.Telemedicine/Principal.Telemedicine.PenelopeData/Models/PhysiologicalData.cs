using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Helper class of physiological data.
/// </summary>
public class PhysiologicalData
{
    /// <summary>
    /// Link to dbo.PhysiologicalDataType as a kind of measured physiological data
    /// </summary>
    [JsonProperty("PhysiologicalDataTypeId")]
    public int PhysiologicalDataTypeId { get; set; }

    /// <summary>
    /// Collection of specific physiological data values
    /// </summary>
    [JsonProperty("Values")]
    public List<Values> Values { get; set; }
}

