using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.Extensions.AI;

namespace LawFirmsHelper.Services;

public class AiAssistantService : IAiAssistantService
{
    private readonly IChatClient _chatClient;
    private readonly ILeadService _leadService;
    
    private readonly IConfiguration _configuration;

    public AiAssistantService(IChatClient chatClient, ILeadService leadService, IConfiguration configuration)
    {
        _chatClient = chatClient;
        _leadService = leadService;
        _configuration = configuration;
    }

    public async Task<string?> GetNextResponseAsync(Guid firmId, List<Message> dbHistory,
        CancellationToken cancellationToken = default)
    {
        var promptLines = _configuration.GetSection("AiSettings:SystemPrompt").Get<string[]>();
        
        var systemPrompt = string.Join("\n", promptLines);
        
        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt)
        };

        chatHistory.AddRange(dbHistory.Select(m => new ChatMessage(
            m.Actor.Type == ActorType.Agent ? ChatRole.Assistant : ChatRole.User, m.Text)));

        var options = new ChatOptions
        {
            Tools =
            [
                AIFunctionFactory.Create(async (string name, string phone, string problemDescription) =>
                    {
                        var request = new CreateLeadRequest
                        {
                            FirmId = firmId,
                            Name = name,
                            Phone = phone,
                            Description = problemDescription
                        };

                        await _leadService.CreateAsync(request, cancellationToken);
                        return "Lead successfully created.";
                    },
                    name: "CreateLead",
                    description: "Зберігає інформацію про ліда в базі даних")
            ]
        };
        var response = await _chatClient.GetResponseAsync(chatHistory, options, cancellationToken);
        return response.Text;
    }
}