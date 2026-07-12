using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface ISubscriptionService
{
    Task<bool> UpdateSubscriptionAsync(Guid firmId, string ownerId, UpdateSubscriptionRequest request, CancellationToken cancellationToken);
}