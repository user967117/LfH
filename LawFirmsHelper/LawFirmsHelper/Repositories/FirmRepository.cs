using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Repositories;

public class FirmRepository : Repository<Firm>, IFirmRepository
{
    private readonly AppDbContext _context;

    public FirmRepository(AppDbContext context) : base(context)
    {
        _context =  context;
    }

    public async Task<Firm?> GetFirmAsync(Guid firmId, string ownerId, CancellationToken cancellationToken)
    {
        return await _context.Firm.Include(f => f.Subscription)
            .FirstOrDefaultAsync(f => f.Id == firmId && f.OwnerId == ownerId, cancellationToken);
    }
    
    public async Task<Firm?> GetFirmWithSubscriptionAndLeadsAsync(Guid firmId, CancellationToken cancellationToken = default)
    {
        return await _context.Firm
            .Include(f => f.Subscription)
            .ThenInclude(s => s.Plan)
            .Include(f => f.Leads) // Завантажуємо лідів, щоб порахувати їх кількість
            .FirstOrDefaultAsync(f => f.Id == firmId, cancellationToken);
    }
}