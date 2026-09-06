using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface ILeadService
{
    public Task<LeadResponse> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken = default);
    
    public Task<Lead?> GetByFirmIdAndEmailAsync(Guid firmId, string email, CancellationToken cancellationToken = default);
}