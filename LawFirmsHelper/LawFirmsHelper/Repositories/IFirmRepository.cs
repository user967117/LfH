using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface IFirmRepository : IRepository<Firm>
{
    Task<Firm?> GetFirmWithSubscriptionAsync(Guid firmId, string ownerId, CancellationToken cancellationToken);
}