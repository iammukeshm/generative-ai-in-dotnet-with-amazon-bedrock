using System.Text.Json.Serialization;

namespace AmazonBedRock.Demo.Models.Cohere;

public class CohereResponse
{
    [JsonPropertyName("generations")]
    public Generation[]? Generations { get; set; }
}

public class Generation
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}