using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class FirmService : IFirmService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserContextService _userContextService;

    public FirmService(AppDbContext dbContext, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    public async Task<Firm> CreateAsync(CreateFirmRequest request,
        CancellationToken cancellationToken = default)
    {
        var context = _userContextService.GetContext();
        var userId = context.UserId;

        var firm = new Firm
        {
            Name = request.Name,
            OwnerId = userId
        };
        
        await _dbContext.Firm.AddAsync(firm, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return firm;
    }

    public async Task<List<Firm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Firm.ToListAsync(cancellationToken);
    }
    
}