using AIDemo.ApiService.Features.Weather.Agents;
using AIDemo.ApiService.Features.Weather.Models;
using System.Runtime.CompilerServices;

namespace AIDemo.ApiService.Features.Weather.Endpoints;

public sealed class WeatherRequestHandler
{
    private readonly WeatherAgentFactory _weatherAgentFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherRequestHandler> _logger;

    public WeatherRequestHandler(WeatherAgentFactory weatherAgentFactory, IConfiguration configuration, ILogger<WeatherRequestHandler> logger)
    {
        _weatherAgentFactory = weatherAgentFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> HandleAsync(WeatherRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var endpointUrl = _configuration["WeatherAgentSettings:EndpointUrl"];
        var apiKey = _configuration["WeatherAgentSettings:ApiKey"];
        var model = _configuration["WeatherAgentSettings:Model"];

        if (string.IsNullOrEmpty(endpointUrl) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(model))
        {
            _logger.LogError("Weather agent configuration is missing required settings.");
            yield return "Error: Weather agent is not properly configured.";
            yield break;
        }

        var agent = _weatherAgentFactory.Create(endpointUrl, apiKey, model);

        await foreach (var update in agent.RunStreamingAsync(request.Prompt, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrWhiteSpace(update.Text))
            {
                yield return update.Text;
            }
        }
    }
}