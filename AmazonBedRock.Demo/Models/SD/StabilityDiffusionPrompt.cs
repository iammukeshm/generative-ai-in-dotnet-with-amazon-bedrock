using System.Text.Json.Serialization;

namespace AmazonBedRock.Demo.Models.SD;

public class StabilityDiffusionPrompt
{
    public StabilityDiffusionPrompt(string prompt)
    {
        var textPrompt = new TextPrompt()
        {
            Text = prompt,
        };
        TextPrompts.Add(textPrompt);
    }
    [JsonPropertyName("text_prompts")]
    public List<TextPrompt> TextPrompts { get; set; } = new List<TextPrompt>();
    [JsonPropertyName("cfg_scale")]
    public float CFGScale { get; set; } = 20F;
    [JsonPropertyName("steps")]
    public int Steps { get; set; } = 50;

}

public class TextPrompt
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("weight")]
    public float Weight { get; set; } = 1F;
}