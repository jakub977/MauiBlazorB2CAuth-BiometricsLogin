using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

public class PhysiologicalDataRoot
{
    [JsonProperty("GlobalId")]
    public string GlobalId { get; set; }

    [JsonProperty("PhysiologicalData")]
    public List<PhysiologicalData> PhysiologicalData { get; set; }
}

