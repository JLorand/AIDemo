using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;

namespace AIDemo.ApiService.Features.Vision.Agents;

public class VisionAgentFactory
{
    public AIAgent Create(string endpointUrl, string apiKey, string model)
    {
        return new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions { Endpoint = new Uri(endpointUrl) })
            .GetChatClient(model)
            .AsAIAgent(
                new ChatClientAgentOptions
                {
                    Name = "VisionAgent",
                    ChatOptions = new ChatOptions
                    {
                        Instructions =
                            """
                            You are a helpful image understanding assistant that can analyze PNG images.
                            The user provides an image as a data URI and optionally a prompt. Describe the contents clearly and concisely.
                            """,
                        Reasoning = new ReasoningOptions
                        {
                            Effort = ReasoningEffort.Medium,
                            Output = ReasoningOutput.Full
                        },
                        Temperature = 0.0f,
                        ResponseFormat = Microsoft.Extensions.AI.ChatResponseFormat.Text,
                        ToolMode = ChatToolMode.Auto
                    }
                })
            .AsBuilder()
            .UseOpenTelemetry("VisionAgent", telemetryAgent => telemetryAgent.EnableSensitiveData = true)
            .Build();
    }
}