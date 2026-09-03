using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

namespace LawFirmsHelper.Extentions;

public static class AiServiceExtentions
{
    public static IServiceCollection AddAiChatClient(this IServiceCollection services, IConfiguration configuration)
    {
        var aiKey = configuration["AiSettings:OpenAIKey"];
        var modelName = configuration["AiSettings:ModelName"];
        var baseUrl = configuration["AiSettings:BaseUrl"];

        services.AddChatClient(sp =>
        {
            var options = new OpenAIClientOptions();
            if (!string.IsNullOrEmpty(baseUrl))
            {
                options.Endpoint = new Uri(baseUrl);
            }

            return new OpenAIClient(new ApiKeyCredential(aiKey), options)
                .GetChatClient(modelName)
                .AsIChatClient();
        });
        return services;
    }
}