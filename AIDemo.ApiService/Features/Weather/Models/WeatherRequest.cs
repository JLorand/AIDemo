using System.ComponentModel.DataAnnotations;

namespace AIDemo.ApiService.Features.Weather.Models;

public record WeatherRequest
{
    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Prompt { get; set; } = string.Empty;
}