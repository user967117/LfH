using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Repositories;

public class LeadRepository
{
    private readonly AppDbContext _context;
    
    public LeadRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Lead> AddAsync(Lead lead, CancellationToken cancellationToken = default)
    {
        await _context.Leads.AddAsync(lead, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return lead;
    }

    public async Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Leads.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
}