using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface ISubscriptionService
{
    Task<bool> AddSubscriptionAsync(UpdateSubscriptionCommand command, CancellationToken cancellationToken = default);
}