using System.ComponentModel.DataAnnotations;

namespace AIDemo.ApiService.Features.Vision.Models;

public record VisionRequest
{
    [Required]
    public IFormFile ImageFile { get; init; } = default!;

    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Prompt { get; init; } = "What do you see in this image?";
}