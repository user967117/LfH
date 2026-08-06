using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IChatService
{
    public Task<ChatResponse> CreateAsync(CreateChatRequest request, CancellationToken cancellationToken = default);
    public Task<List<ChatMessageResponse>> GetChatHistoryAsync(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default);
}