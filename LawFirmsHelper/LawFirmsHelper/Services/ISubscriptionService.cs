using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public interface ISubscriptionService
{
    Task<bool> AddOrUpdateAsync(AddOrUpdateSubscriptionCommand command, CancellationToken cancellationToken = default);
}