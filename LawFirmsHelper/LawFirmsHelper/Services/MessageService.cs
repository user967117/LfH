using LawFirmsHelper.Extentions;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class MessageService : IMessageService
{
    public readonly IMessageRepository _messageRepository;
    public readonly ILeadRepository _leadRepository;

    public MessageService(IMessageRepository messageRepository, ILeadRepository leadRepository)
    {
        _messageRepository = messageRepository;
        _leadRepository = leadRepository;
    }

    public async Task<MessageResponse> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken)
    {
        var leadExist = await _leadRepository.GetByIdAsync(request.LeadId, cancellationToken);

        if (leadExist == null) return null;

        var message = request.ToMessage();

        await _messageRepository.AddAsync(message);
        await _messageRepository.SaveChangesAsync(cancellationToken);

        return message.ToResponse();
    }
}