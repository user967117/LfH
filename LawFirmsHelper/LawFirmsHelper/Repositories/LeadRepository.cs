using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Repositories;

public class LeadRepository : Repository<Lead>, ILeadRepository
{
    private readonly AppDbContext _context;
    
    public LeadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Lead> AddAsync(Lead lead, CancellationToken cancellationToken = default)
    {
        await _context.Leads.AddAsync(lead, cancellationToken);
        return lead;
    }

    public async Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Leads.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    
    public async Task<int> GetCountAsync(Guid firmId, CancellationToken cancellationToken = default)
    {
        return await _context.Leads.CountAsync(l => l.FirmId == firmId, cancellationToken);
    }
}