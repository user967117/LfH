using LawFirmsHelper.Extentions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class LeadService : ILeadService
{
    private readonly AppDbContext _context;
    public LeadService(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<LeadResponse> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken = default)
    {
        var firm = await _context.Firm.Include(f => f.Subscription)
            .ThenInclude(s => s.Plan)
            .Include(f => f.Leads).FirstOrDefaultAsync(f => f.Id == request.FirmId);

        if (firm == null) 
            throw new ArgumentException($"Firm {request.FirmId} does not exist");
        

        if (firm.Leads.Count > firm.Subscription.Plan.MaxLeads)
            throw new ArgumentException($"Firm {request.FirmId} does not have enough leads");

        var lead = request.ToLead();
        
        await _context.Leads.AddAsync(lead);
        await _context.SaveChangesAsync(cancellationToken);
        
        return lead.ToResponse();
    }
    
    
}