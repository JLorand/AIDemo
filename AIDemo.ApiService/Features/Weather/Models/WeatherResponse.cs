using System.Text.Json.Serialization;

namespace AIDemo.ApiService.Features.Weather.Models;

public record WeatherResponse
{
    [JsonPropertyName("current")]
    public CurrentWeather? Current { get; set; }
}

public record CurrentWeather
{
    [JsonPropertyName("temperature_2m")]
    public double? Temperature2m { get; set; }
}
