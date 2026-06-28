using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface IFirmService
{
    Task<Firms> CreateAsync(string ownerId, CreateFirmRequest request, CancellationToken cancellationToken = default);
}