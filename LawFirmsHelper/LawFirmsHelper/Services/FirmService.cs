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

    public FirmService(AppDbContext dbContext, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }
    

    public async Task<Firm> CreateAsync(CreateFirmRequest request,
        CancellationToken cancellationToken = default)
    {
        var firmExists = await _dbContext.Firm.AnyAsync(cancellationToken);

        if (firmExists)
        {
            throw new InvalidOperationException($"Firm {request.Name} already exist"); 
        }
        
        var context = _userContextService.GetContext();
        var userId = context.UserId;

        var firm = new Firm
        {
            Name = request.Name,
            OwnerId = userId
        };
        
        await _firmRepository.AddAsync(firm, cancellationToken);
        await _firmRepository.SaveChangesAsync(cancellationToken);
        
        return firm;
    }

    public async Task<List<Firm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Firm.ToListAsync(cancellationToken);
    }
    
}