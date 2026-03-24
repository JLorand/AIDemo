
using Scalar.AspNetCore;
using AIDemo.ApiService.Features.Weather.Endpoints;
using AIDemo.ApiService.Features.Weather.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add console logging in addition to Aspire OpenTelemetry hooks.
builder.Logging.AddConsole();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddWeatherFeature(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "API service is running.");

app.MapWeatherEndpoints();

app.MapDefaultEndpoints();

app.Run();
