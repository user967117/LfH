using LawFirmsHelper.Models;

namespace LawFirmsHelper.Services;

public interface IAiAssistantService
{
    Task<string?> GetNextResponseAsync(Guid FirmId, Guid chatId, List<Message> dbHistory, CancellationToken cancellationToken = default);
}