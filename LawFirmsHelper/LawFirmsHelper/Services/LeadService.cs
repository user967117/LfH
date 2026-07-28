using LawFirmsHelper.Extentions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class LeadService : ILeadService
{
    private readonly ILeadRepository _leadRepository;
    private readonly IFirmRepository _firmRepository;
    public LeadService(ILeadRepository leadRepository, IFirmRepository firmRepository)
    {
        _firmRepository = firmRepository;
        _leadRepository = leadRepository;
    }

    public async Task<LeadResponse> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken = default)
    {
        var firm = await _firmRepository.GetFirmWithLeadsAsync(request.FirmId, cancellationToken);

        if (firm == null) 
            throw new ArgumentException($"Firm {request.FirmId} does not exist");
        

        if (firm.Leads.Count > firm.Subscription.Plan.MaxLeads)
            throw new ArgumentException($"Firm {request.FirmId} does not have enough leads");

        var lead = request.ToLead();
        
        await _leadRepository.AddAsync(lead);
        await _firmRepository.SaveChangesAsync(cancellationToken);
        
        return lead.ToResponse();
    }
    
    
}