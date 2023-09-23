using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

public class Values
{
    [JsonProperty("MeasuredValue")]
    public string MeasuredValue { get; set; }

    [JsonProperty("MeasuredTime")]
    public DateTime MeasuredTime { get; set; }

    [JsonProperty("PhysiologicalDataCreationTypeId")]
    public int? PhysiologicalDataCreationTypeId { get; set; }
}

