using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.Extensions.AI;

namespace LawFirmsHelper.Services;

public class AiAssistantService : IAiAssistantService
{
    private readonly IChatClient _chatClient;
    private readonly ILeadService _leadService;

    public AiAssistantService(IChatClient chatClient, ILeadService leadService)
    {
        _chatClient = chatClient;
        _leadService = leadService;
    }

    public async Task<string?> GetNextResponseAsync(Guid firmId, List<Message> dbHistory,
        CancellationToken cancellationToken = default)
    {
        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, """
                Ти асистент юридичної фірми.
                Твоя мета зібрати інформацію для передачі фірмі:
                1. Ім'я клієнта
                2. Електронну пошту
                3. Номер телефону
                4. Короткий опис проблеми
                
                Став лише одне запитання за раз.
                Коли збереш усю інформацію, виклич метод CreateLead.
            """)
        };

        foreach (var message in dbHistory)
        {
            var role = message.Actor.Type == ActorType.Agent ? ChatRole.Assistant : ChatRole.User;
            chatHistory.Add(new ChatMessage(role, message.Text));
        }

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