using System.ComponentModel;
using AIDemo.ApiService.Features.Weather.Services;

namespace AIDemo.ApiService.Features.Weather.Tools;

public class WeatherTool
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherTool> _logger;

    public WeatherTool(IWeatherService weatherService, ILogger<WeatherTool> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [Description("Gets the current weather for a given location.")]
    public async Task<string> CurrentWeather(
        [Description("The latitude of the location.")] string latitude,
        [Description("The longitude of the location.")] string longitude,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(latitude) || string.IsNullOrWhiteSpace(longitude))
        {
            _logger.LogWarning("CurrentWeather called with empty latitude or longitude.");
            throw new ArgumentException("Latitude and longitude are required.");
        }

        _logger.LogInformation("Requesting current weather for {Latitude},{Longitude}.", latitude, longitude);

        var weather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);

        if (weather.Current?.Temperature2m is not double temp)
        {
            _logger.LogError("Weather data is missing temperature information.");
            return "Unable to retrieve temperature data for the specified location.";
        }

        _logger.LogInformation("Current temperature for {Latitude},{Longitude} is {Temperature}°C.", latitude, longitude, temp);

        return $"{temp:F1} degrees Celsius";
    }
}
