using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface IFirmRepository : IRepository<Firm>
{
    Task<Firm?> GetFirmAsync(Guid firmId, string ownerId, CancellationToken cancellationToken);
    Task<Firm?> GetFirmWithLeadsAsync(Guid firmId, CancellationToken cancellationToken);
}