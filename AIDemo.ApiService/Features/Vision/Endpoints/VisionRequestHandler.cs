using AIDemo.ApiService.Features.Vision.Agents;
using AIDemo.ApiService.Features.Vision.Models;
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace AIDemo.ApiService.Features.Vision.Endpoints;

public sealed class VisionRequestHandler
{
    private readonly VisionAgentFactory _visionAgentFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<VisionRequestHandler> _logger;

    public VisionRequestHandler(VisionAgentFactory visionAgentFactory, IConfiguration configuration, ILogger<VisionRequestHandler> logger)
    {
        _visionAgentFactory = visionAgentFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> HandleAsync(VisionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var endpointUrl = _configuration["VisionAgentSettings:EndpointUrl"];
        var apiKey = _configuration["VisionAgentSettings:ApiKey"];
        var model = _configuration["VisionAgentSettings:Model"];

        if (string.IsNullOrEmpty(endpointUrl) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(model))
        {
            _logger.LogError("Vision agent configuration is missing required settings.");
            yield return "Error: Vision agent is not properly configured.";
            yield break;
        }

        await using var stream = request.ImageFile.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, cancellationToken);
        byte[] imageBytes = memory.ToArray();

        string mimeType = request.ImageFile.ContentType; 

        string base64String = Convert.ToBase64String(imageBytes);
        string dataUri = $"data:{mimeType};base64,{base64String}";

        var message = new ChatMessage(
            ChatRole.User,
            [
                new TextContent(request.Prompt),
                new DataContent(dataUri) 
            ]);

        var agent = _visionAgentFactory.Create(endpointUrl, apiKey, model);

        await foreach (var update in agent.RunStreamingAsync(message, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrWhiteSpace(update.Text))
            {
                yield return update.Text;
            }
        }
    }
}