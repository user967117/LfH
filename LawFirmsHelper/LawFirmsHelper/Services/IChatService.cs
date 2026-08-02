using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IChatService
{
    public Task<ChatResponse> CreateAsync(CreateChatRequest request, CancellationToken cancellationToken = default);
}