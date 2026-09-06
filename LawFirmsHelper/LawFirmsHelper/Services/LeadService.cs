using System.ComponentModel.DataAnnotations;
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
        
        var currentLeadsCount = await _leadRepository.GetCountAsync(request.FirmId, cancellationToken);

        if (currentLeadsCount > firm.Subscription.Plan.MaxLeads)
            throw new ValidationException($"Firm {request.FirmId} does not have enough leads");

        var lead = request.ToLead();
        
        await _leadRepository.AddAsync(lead);
        await _leadRepository.SaveChangesAsync(cancellationToken);
        
        return lead.ToResponse();
    }
    
    public async Task<Lead?> GetByFirmIdAndEmailAsync(Guid firmId, string email, CancellationToken cancellationToken = default)
    {
        return await _leadRepository.GetByFirmIdAndEmailAsync(firmId, email, cancellationToken);
    }
}