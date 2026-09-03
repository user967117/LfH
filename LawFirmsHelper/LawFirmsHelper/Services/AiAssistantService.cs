using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace LawFirmsHelper.Services;

public class AiAssistantService : IAiAssistantService
{
    private readonly IChatClient _chatClient;
    private readonly ILeadService _leadService;
    private readonly IChatRepository _chatRepository;
    private readonly AiSettings _aiSettings;

    public AiAssistantService(IChatClient chatClient, ILeadService leadService, IChatRepository chatRepository, IOptions<AiSettings> aiOptions)
    {
        _chatClient = new ChatClientBuilder(chatClient).UseFunctionInvocation().Build();
        _leadService = leadService;
        _chatRepository = chatRepository;
        _aiSettings = aiOptions.Value;
    }
    
    private string FormatModelResponse(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        
        int index = text.LastIndexOf("</think>");
        if (index != -1)
        {
            return text.Substring(index + 8).Trim();
        }   
        return text;
    } 

    public async Task<ChatModelResponse> GetNextResponseAsync(GetModelResponseRequest request,
        CancellationToken cancellationToken = default)
    {
        var promptLines = _aiSettings.SystemPrompt;
        
        var systemPrompt = string.Join("\n", promptLines);
        
        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt)
        };

        chatHistory.AddRange(request.DbHistory.Select(m => new ChatMessage(
            m.Actor.Type == ActorType.Agent ? ChatRole.Assistant : ChatRole.User, m.Text)));

        var createLeadTool = AIFunctionFactory.Create(
            async (string? name, string? phone, string? email, string? description) =>
            {
                var missingFields = new List<string>();
                if (string.IsNullOrWhiteSpace(name)) missingFields.Add("Ім'я");
                if (string.IsNullOrWhiteSpace(phone)) missingFields.Add("Телефон");
                if (string.IsNullOrWhiteSpace(email)) missingFields.Add("Email");
                if (string.IsNullOrWhiteSpace(description)) missingFields.Add("Опис проблеми");

                if (missingFields.Any())
                {
                    var missingStr = string.Join(", ", missingFields);
                    return $"SYSTEM ERROR: Missing required fields: {missingStr}. DO NOT call this function yet. Ask the user for the missing information.";
                }
                
                var existingLead = await _leadService.GetByFirmIdAndEmailAsync(request.FirmId, email!, cancellationToken);
                if (existingLead != null)
                {
                    var currentChat = await _chatRepository.GetByIdAsync(request.ChatId, cancellationToken);
                    if (currentChat != null && currentChat.LeadId == null)
                    {
                        currentChat.LeadId = existingLead.Id;
                        await _chatRepository.SaveChangesAsync(cancellationToken);
                    }
                    return "SYSTEM: This lead already exists in the database. DO NOT try to save it again. Just politely answer the user's question.";
                }
            

                var createLeadRequest = new CreateLeadRequest
                {
                    FirmId = request.FirmId,
                    Name = name!,
                    Phone = phone!,
                    Email = email!,
                    Description = description!
                };

                await _leadService.CreateAsync(createLeadRequest, cancellationToken);

                var newLead = await _leadService.GetByFirmIdAndEmailAsync(request.FirmId, email!, cancellationToken);
                if (newLead != null)
                {
                    var currentChat = await _chatRepository.GetByIdAsync(request.ChatId, cancellationToken);
                    if (currentChat != null)
                    {
                        currentChat.LeadId = newLead.Id;
                        await _chatRepository.SaveChangesAsync(cancellationToken);
                    }
                }
                return "Lead successfully created. Now tell the user that their data is saved.";
            },
            name: "CreateLead",
            description: "Зберігає інформацію про ліда. ВАЖЛИВО: Викликай ТІЛЬКИ тоді, коли зібрав УСІ 4 параметри (ім'я, телефон, email, опис проблеми). Якщо чогось бракує НЕ викликай інструмент, а напиши користувачу і запитай.");
        
        var options = new ChatOptions
        {
            Tools = [ createLeadTool ]
        };
        
        var response = await _chatClient.GetResponseAsync(chatHistory, options, cancellationToken);
        return new ChatModelResponse 
        {
            Text = FormatModelResponse(response.Text)
        };
    }
}