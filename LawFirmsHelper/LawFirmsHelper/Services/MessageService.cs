using LawFirmsHelper.Extentions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class MessageService : IMessageService
{
    public readonly IMessageRepository _messageRepository;
    public readonly IChatRepository _chatRepository;
    public readonly ILeadRepository _leadRepository;
    
    private readonly IAiAssistantService _aiAssistantService;

    public MessageService(IMessageRepository messageRepository, IChatRepository chatRepository,  IAiAssistantService aiAssistantService, ILeadRepository leadRepository)
    {
        _messageRepository = messageRepository;
        _chatRepository = chatRepository;
        _aiAssistantService = aiAssistantService;
        _leadRepository = leadRepository;
    }

    public async Task<MessageResponse> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken)
    {
        
        var chatExist = await _chatRepository.GetByIdAsync(request.ChatId, cancellationToken);
        if (chatExist == null) return null;

        var userMessage = request.ToMessage();
        userMessage.Actor = new Actor { Type = ActorType.Lead };

        await _messageRepository.AddAsync(userMessage);
        await _messageRepository.SaveChangesAsync(cancellationToken);
        
        Lead? lead = null;
        if (chatExist.LeadId.HasValue)
        {
            lead = await _leadRepository.GetByIdAsync(chatExist.LeadId.Value, cancellationToken);
        }
        
        
        var chatHistory = await _messageRepository.GetMessageByChatIdAsync(request.ChatId, 0, 50, cancellationToken);

        var aiRequest = new GetModelResponseRequest
        {
            FirmId = chatExist.FirmId,
            ChatId = request.ChatId,
            DbHistory = chatHistory
        };
        
        var aiResponse = await _aiAssistantService.GetNextResponseAsync(aiRequest, cancellationToken);

        var aiMessage = new Message
        {
            ChatId = request.ChatId,
            Text = aiResponse.Text,
            Actor = new Actor { Type = ActorType.Agent },
        };
        
        await _messageRepository.AddAsync(aiMessage);
        await _messageRepository.SaveChangesAsync(cancellationToken);
        
        return aiMessage.ToResponse();
    }
}