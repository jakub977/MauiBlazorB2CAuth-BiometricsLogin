using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Helper class of physiological data.
/// </summary>
public class PhysiologicalDataRoot
{
    /// <summary>
    /// User's global identifier
    /// </summary>
    [JsonProperty("GlobalId")]
    public string GlobalId { get; set; }

    /// <summary>
    /// Collection of user's specific physiological data 
    /// </summary>
    [JsonProperty("PhysiologicalData")]
    public List<PhysiologicalData> PhysiologicalData { get; set; }
}

