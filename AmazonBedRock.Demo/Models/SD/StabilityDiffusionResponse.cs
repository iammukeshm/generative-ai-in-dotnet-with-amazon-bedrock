using System.Text.Json.Serialization;

namespace AmazonBedRock.Demo.Models.SD;

public class StabilityDiffusionResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; }
    [JsonPropertyName("artifacts")]
    public List<Artifact> Artifacts { get; set; }
}

public class Artifact
{
    [JsonPropertyName("base64")]
    public string Base64 { get; set; }
    [JsonPropertyName("finishReason")]
    public string FinishReason { get; set; }
}