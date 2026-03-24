using AIDemo.ApiService.Features.Weather.Models;

namespace AIDemo.ApiService.Features.Weather.Services;

public interface IWeatherService
{
    Task<WeatherResponse> GetCurrentWeatherAsync(string latitude, string longitude, CancellationToken cancellationToken);
}
