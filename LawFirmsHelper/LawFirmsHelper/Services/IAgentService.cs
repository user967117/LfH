using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IAgentService
{
    Task<AgentResponse> CreateAsync(CreateAgentRequest request, CancellationToken cancellationToken = default);
    Task<List<AgentResponse>> GetAllByFirmIdAsync(Guid firmId, CancellationToken cancellationToken = default);
}