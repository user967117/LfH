using LawFirmsHelper.Extentions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class AgentService : IAgentService
{
    private readonly AppDbContext _dbContext;
    private readonly IRepository<Agent> _agentRepository;
    private readonly IRepository<Firm> _firmRepository;
    public AgentService(IRepository<Agent> agentRepository, IRepository<Firm> firmRepository)
    {
        _agentRepository = agentRepository;
        _firmRepository = firmRepository;
    }

    public async Task<AgentResponse> CreateAsync(CreateAgentRequest request, CancellationToken cancellationToken = default)
    {
        var firmExist = await _firmRepository.AnyAsync(f => f.Id == request.FirmId, cancellationToken);

        if (!firmExist)
        {
            throw new ArgumentException($"Firm {request.FirmId} not found");
        }

        var agent = new Agent
        {
            Name = request.Name,
            FirmId = request.FirmId,
            Status = AgentStatus.Active
        };
        
        await _agentRepository.AddAsync(agent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AgentResponse
        {
            Id = agent.Id,
            Name = agent.Name,
            Status = agent.Status,
            FirmId = agent.FirmId
        };
    }

    public async Task<List<AgentResponse>> GetAllByFirmIdAsync(Guid firmId, CancellationToken cancellationToken = default)
    {
        var agents = await _agentRepository.GetWhereAsync(a => a.FirmId == firmId, cancellationToken);

        return agents.ToResponse();
    }
}