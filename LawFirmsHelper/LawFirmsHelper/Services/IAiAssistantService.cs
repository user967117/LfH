using LawFirmsHelper.Models;

namespace LawFirmsHelper.Services;

public interface IAiAssistantService
{
    Task<string?> GetNextResponseAsync(Guid FirmId, List<Message> dbHistory, CancellationToken cancellationToken = default);
}