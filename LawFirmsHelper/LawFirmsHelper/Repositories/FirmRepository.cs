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

    public async Task<Firm?> GetFirmWithSubscriptionAsync(Guid firmId, string ownerId, CancellationToken cancellationToken)
    {
        return await _context.Firm.Include(f => f.Subscriptions)
            .FirstOrDefaultAsync(f => f.Id == firmId && f.OwnerId == ownerId, cancellationToken);
    }
}