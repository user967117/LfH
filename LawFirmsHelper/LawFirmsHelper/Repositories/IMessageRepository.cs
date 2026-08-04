using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<List<Message>> GetByLeadIdAsync(Guid leadId, CancellationToken cancellationToken = default);

    Task<List<Message>> GetMessageByChatIdAsync(Guid chatId, CancellationToken cancellationToken);
}