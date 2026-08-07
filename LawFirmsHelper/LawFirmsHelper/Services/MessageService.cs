using LawFirmsHelper.Extentions;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class MessageService : IMessageService
{
    public readonly IMessageRepository _messageRepository;
    public readonly IChatRepository _chatRepository;

    public MessageService(IMessageRepository messageRepository, IChatRepository chatRepository)
    {
        _messageRepository = messageRepository;
        _chatRepository = chatRepository;
    }

    public async Task<MessageResponse> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken)
    {
        var chatExist = await _chatRepository.GetByIdAsync(request.ChatId, cancellationToken);

        if (chatExist == null) return null;

        var message = request.ToMessage();

        await _messageRepository.AddAsync(message);
        await _messageRepository.SaveChangesAsync(cancellationToken);

        return message.ToResponse();
    }
}