using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

public class PhysiologicalData
{
    [JsonProperty("PhysiologicalDataTypeId")]
    public int PhysiologicalDataTypeId { get; set; }

    [JsonProperty("Values")]
    public List<Values> Values { get; set; }
}

