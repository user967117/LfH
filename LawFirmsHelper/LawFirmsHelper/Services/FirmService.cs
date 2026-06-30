using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class FirmService : IFirmService
{
    private readonly AppDbContext _dbContext;

    public FirmService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Firm> CreateAsync(Firm firm, CreateFirmRequest request,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Firm.AddAsync(firm, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return firm;
    }

    public async Task<List<Firm>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Firm.ToListAsync(cancellationToken);
    }

    public async Task<Firm> CreateAsync(string ownerId, CreateFirmRequest request, CancellationToken cancellationToken = default)
    {
        var firm = new Firm
        {
            Name = request.Name,
            OwnerId = ownerId
        };
        await _dbContext.Firm.AddAsync(firm, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return firm;

    }
}