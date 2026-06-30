using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IFirmService
{
    Task<Firm> CreateAsync(string ownerId, CreateFirmRequest request, CancellationToken cancellationToken = default);
}