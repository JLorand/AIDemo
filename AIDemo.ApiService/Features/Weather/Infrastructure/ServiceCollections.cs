using AIDemo.ApiService.Features.Weather.Agents;
using AIDemo.ApiService.Features.Weather.Endpoints;
using AIDemo.ApiService.Features.Weather.Services;
using AIDemo.ApiService.Features.Weather.Tools;

namespace AIDemo.ApiService.Features.Weather.Infrastructure
{
    public static class ServiceCollections
    {
        public static void AddWeatherFeature(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri(configuration["WeatherApi:BaseUrl"] ?? "https://api.open-meteo.com");
                client.Timeout = TimeSpan.FromSeconds(int.Parse(configuration["WeatherApi:TimeoutSeconds"] ?? "10"));
            });

            services.AddScoped<WeatherTool>();
            services.AddScoped<WeatherAgentFactory>();
            services.AddScoped<WeatherRequestHandler>();
        }
    }
}