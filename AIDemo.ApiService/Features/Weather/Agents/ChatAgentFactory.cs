using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using AIDemo.ApiService.Features.Weather.Tools;

namespace AIDemo.ApiService.Features.Weather.Agents;

public class WeatherAgentFactory
{
    private readonly IEnumerable<WeatherTool> _weatherTools;

    public WeatherAgentFactory(IEnumerable<WeatherTool> weatherTools)
    {
        _weatherTools = weatherTools;
    }

    public AIAgent Create(string endpointUrl, string apiKey, string model)
    {
        return new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions { Endpoint = new Uri(endpointUrl) })
            .GetChatClient(model)
            .AsAIAgent(
                new ChatClientAgentOptions
                {
                    Name = "WeatherAgent",
                    ChatOptions = new ChatOptions
                    {
                        Instructions = 
                            """
                            You are a helpful weather forecast assistant that provides concise answers to user questions.
                            Only answer weather questions. Use the CurrentWeather tool whenever available.
                            When calling the CurrentWeather tool, make sure to fill both latitude and longitude information for the specified location.
                            """,
                        Reasoning = new ReasoningOptions
                        {
                            Effort = ReasoningEffort.Medium,
                            Output = ReasoningOutput.Full
                        },
                        Temperature = 0.0f,
                        ResponseFormat = Microsoft.Extensions.AI.ChatResponseFormat.Text,
                        ToolMode = ChatToolMode.Auto,
                        Tools = _weatherTools
                            .Select(tool => AIFunctionFactory.Create(tool.CurrentWeather))
                            .ToArray()
                    }
                })
            .AsBuilder()
            .UseOpenTelemetry("WeatherAgent", telemetryAgent => telemetryAgent.EnableSensitiveData = true)
            .Build();
    }
}
