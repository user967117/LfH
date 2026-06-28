using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class FirmService : IFirmService
{
    private readonly List<Firms> _firms = new();
    private readonly Lock _lock = new();

    public Task<Firms> CreateAsync(string ownerId, CreateFirmRequest request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var firm = new Firms
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerID = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        lock (_lock)
        {
            _firms.Add(firm);
        }
        
        return Task.FromResult(firm);
    }
}