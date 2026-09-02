using LawFirmsHelper.Extentions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class FirmService : IFirmService
{
    private readonly AppDbContext _dbContext;
    private readonly IFirmRepository _firmRepository;
    private readonly IUserContextService _userContextService;

    public FirmService(AppDbContext dbContext, IUserContextService userContextService, IFirmRepository firmRepository)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
        _firmRepository = firmRepository;
    }
    

    public async Task<FirmResponse> CreateAsync(CreateFirmRequest request,
        CancellationToken cancellationToken = default)
    {
        var firmExists = await _dbContext.Firm.AnyAsync(f => f.Name == request.Name, cancellationToken);

        if (firmExists)
        {
            throw new InvalidOperationException($"Firm {request.Name} already exist"); 
        }
        
        var context = _userContextService.GetContext();
        var userId = context.UserId;

        var firm = new Firm
        {
            Name = request.Name,
            OwnerId = userId,
        };
        
        var defaultPlan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Name == "Free", cancellationToken);
        if (defaultPlan != null)
        {
            firm.Subscription = new Subscription
            {
                FirmId = firm.Id,
                PlanId = defaultPlan.Id,
                CreatedAt = DateTime.UtcNow
            };
        }
        
        await _firmRepository.AddAsync(firm, cancellationToken);
        await _firmRepository.SaveChangesAsync(cancellationToken);
        
        return firm.ToResponse();
    }

    public async Task<List<Firm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Firm.ToListAsync(cancellationToken);
    }
    
}