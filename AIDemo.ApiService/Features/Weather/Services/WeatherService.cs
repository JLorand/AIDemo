using System.Text.Json;
using System.Text.Json.Serialization;
using AIDemo.ApiService.Features.Weather.Models;

namespace AIDemo.ApiService.Features.Weather.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WeatherResponse> GetCurrentWeatherAsync(string latitude, string longitude, CancellationToken cancellationToken)
    {
        var requestUri = $"/v1/forecast?latitude={Uri.EscapeDataString(latitude)}&longitude={Uri.EscapeDataString(longitude)}&current=temperature_2m";

        _logger.LogDebug("Calling weather API: {RequestUri}", requestUri);

        var json = await _httpClient.GetStringAsync(requestUri, cancellationToken);

        var result = JsonSerializer.Deserialize<WeatherResponse>(json, JsonOptions);

        if (result?.Current?.Temperature2m is null)
        {
            _logger.LogError("Weather API response validation failed: {ResponseBody}", json);
            throw new InvalidOperationException("Invalid response from weather API: current temperature missing.");
        }

        _logger.LogInformation("Weather API returned {Temperature}°C", result.Current.Temperature2m);

        return result;
    }
}
