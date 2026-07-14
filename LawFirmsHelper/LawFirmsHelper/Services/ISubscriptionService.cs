using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface ISubscriptionService
{
    Task<bool> UpdateSubscriptionAsync(UpdateSubscriptionRequest request, CancellationToken cancellationToken = default);
}