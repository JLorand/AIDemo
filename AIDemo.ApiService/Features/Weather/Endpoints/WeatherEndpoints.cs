using AIDemo.ApiService.Features.Weather.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIDemo.ApiService.Features.Weather.Endpoints;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this WebApplication app)
    {
        var weatherGroup = app.MapGroup("/weather")
            .WithTags("Weather");

        weatherGroup
            .MapPost("/chat", async ([FromBody] WeatherRequest request, WeatherRequestHandler weatherAgentHandler, CancellationToken cancellationToken) =>
            {
                var stream = weatherAgentHandler.HandleAsync(request, cancellationToken);
                return TypedResults.ServerSentEvents(stream);
            })
            .WithName("ChatWithWeatherAgent")
            .WithSummary("Chat with AI weather assistant")
            .WithDescription("Send a message to the AI weather assistant and receive streaming responses.");
    }
}