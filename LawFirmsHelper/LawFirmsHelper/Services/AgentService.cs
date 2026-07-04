using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class AgentService : IAgentService
{
    private readonly AppDbContext _dbContext;
    public AgentService(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Agent> CreateAsync(CreateAgentRequest request, CancellationToken cancellationToken = default)
    {
        var firmExist = await _dbContext.Firm.AnyAsync(f => f.Id == request.FirmId, cancellationToken);

        if (!firmExist)
        {
            throw new ArgumentException($"Firm {request.FirmId} not found");
        }

        var agent = new Agent
        {
            Name = request.Name,
            FirmId = request.FirmId,
            IsActive = true
        };
        
        await _dbContext.Agents.AddAsync(agent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return agent;
    }

    public async Task<List<Agent>> GetAllByFirmIdAsync(Guid firmId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Agents.Where(f => f.FirmId == firmId).ToListAsync(cancellationToken);   
    }
}