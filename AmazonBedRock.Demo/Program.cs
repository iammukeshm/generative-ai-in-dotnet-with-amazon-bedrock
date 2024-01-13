using Amazon.BedrockRuntime;
using AmazonBedRock.Demo.Models;
using AmazonBedRock.Demo.Models.Cohere;
using AmazonBedRock.Demo.Models.SD;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<AmazonBedrockRuntimeClient>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//apis
app.MapPost("/prompts/text", async (AmazonBedrockRuntimeClient client, TextPromptRequest request) =>
{
    var coherePrompt = new CoherePrompt(request.Prompt);
    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(coherePrompt));
    var stream = new MemoryStream(bytes);
    var requestModel = new Amazon.BedrockRuntime.Model.InvokeModelRequest()
    {
        ModelId = "cohere.command-text-v14",
        ContentType = "application/json",
        Accept = "*/*",
        Body = stream
    };
    var response = await client.InvokeModelAsync(requestModel);

    var data = JsonSerializer.Deserialize<CohereResponse>(response.Body);
    return new TextPromptReponse(data!.Generations![0].Text!.Trim());
});

app.MapPost("/prompts/image", async (AmazonBedrockRuntimeClient client, ImagePromptRequest request) =>
{
    var sdPrompt = new StabilityDiffusionPrompt(request.Prompt);
    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sdPrompt));
    var stream = new MemoryStream(bytes);
    var requestModel = new Amazon.BedrockRuntime.Model.InvokeModelRequest()
    {
        ModelId = "stability.stable-diffusion-xl-v1",
        ContentType = "application/json",
        Accept = "*/*",
        Body = stream
    };
    var response = await client.InvokeModelAsync(requestModel);

    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "genai-images");
    Directory.CreateDirectory(pathToSave);

    var data = JsonSerializer.Deserialize<StabilityDiffusionResponse>(response.Body);
    foreach (var artifact in data!.Artifacts)
    {
        int suffix = 1;
        var imageBytes = Convert.FromBase64String(artifact.Base64);
        var fileName = $"{request.Prompt.Replace(' ', '-')}-{suffix}.png";
        var filePath = Path.Combine(pathToSave, fileName);
        File.WriteAllBytes(filePath, imageBytes);
        suffix++;
    }
    return Results.Ok();
});


app.Run();