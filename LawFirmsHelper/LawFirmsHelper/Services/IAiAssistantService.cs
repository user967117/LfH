using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IAiAssistantService
{
    Task<ChatModelResponse> GetNextResponseAsync(GetModelResponseRequest request, CancellationToken cancellationToken = default);
}