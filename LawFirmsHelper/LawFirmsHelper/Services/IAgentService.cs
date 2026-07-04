using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IAgentService
{
    Task<Agent> CreateAsync(CreateAgentRequest request, CancellationToken cancellationToken = default);
    Task<List<Agent>> GetAllByFirmIdAsync(Guid firmId, CancellationToken cancellationToken = default);
}