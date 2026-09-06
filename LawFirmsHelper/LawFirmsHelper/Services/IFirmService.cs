using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IFirmService
{
    Task<FirmResponse> CreateAsync(CreateFirmRequest request, CancellationToken cancellationToken = default);
}