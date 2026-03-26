using AIDemo.ApiService.Features.Vision.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIDemo.ApiService.Features.Vision.Endpoints;

public static class VisionEndpoints
{
    public static void MapVisionEndpoints(this WebApplication app)
    {
        var visionGroup = app.MapGroup("/vision")
            .WithTags("Vision");

        visionGroup
            .MapPost("/image-upload", async ([FromForm] VisionRequest request, VisionRequestHandler visionRequestHandler, CancellationToken cancellationToken) =>
            {
                var stream = visionRequestHandler.HandleAsync(request, cancellationToken);
                return TypedResults.ServerSentEvents(stream);
            })
            .WithName("VisionImageUpload")
            .WithSummary("Analyze an image file with AI")
            .WithDescription("Upload an image file and receive streaming AI analysis.")
            .DisableAntiforgery();
    }
}