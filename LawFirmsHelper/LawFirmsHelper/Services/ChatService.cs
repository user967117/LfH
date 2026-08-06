using LawFirmsHelper.Extentions;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class ChatService : IChatService
{
    public readonly IChatRepository _chatRepository;
    public readonly ILeadRepository _leadRepository;
    public readonly IMessageRepository _messageRepository;

    public ChatService(IChatRepository chatRepository, ILeadRepository leadRepository)
    {
        _chatRepository = chatRepository;
        _leadRepository = leadRepository;
    }

    public async Task<ChatResponse> CreateAsync(CreateChatRequest request, CancellationToken cancellationToken = default)
    {
        var leadExist = await _leadRepository.GetByIdAsync(request.LeadId, cancellationToken)!=null;

        if (!leadExist) return null;

        var chat = request.ToChat();
        

        await _chatRepository.AddAsync(chat);
        await _chatRepository.SaveChangesAsync(cancellationToken);

        return chat.ToResponse();
    }
    public async Task<List<ChatMessageResponse>> GetChatHistoryAsync(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default)
    {
        var messages = await _messageRepository.GetMessageByChatIdAsync(chatId, offset, limit, cancellationToken);
        
        return messages.Select(m => m.ToChatHistoryResponse()).ToList();
    }
}