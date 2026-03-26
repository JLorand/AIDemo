using AIDemo.ApiService.Features.Vision.Agents;
using AIDemo.ApiService.Features.Vision.Endpoints;

namespace AIDemo.ApiService.Features.Vision.Infrastructure;

public static class ServiceCollections
{
    public static void AddVisionFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VisionAgentFactory>();
        services.AddScoped<VisionRequestHandler>();
    }
}