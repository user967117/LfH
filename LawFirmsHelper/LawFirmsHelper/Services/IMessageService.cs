using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IMessageService
{
    public Task<MessageResponse> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken);
}