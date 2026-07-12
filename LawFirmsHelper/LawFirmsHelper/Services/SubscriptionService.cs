using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<SubscriptionPlan> _subscriptionRepository;
    private readonly IRepository<Firm> _firmRepository;
    
    public SubscriptionService(IRepository<SubscriptionPlan> subscriptionRepository, IRepository<Firm> firmRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _firmRepository = firmRepository;
    }

    public async Task<bool> UpdateSubscriptionAsync(Guid firmId, string ownerId, UpdateSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var firms = await _firmRepository.GetWhereAsync(f => f.Id == firmId && f.OwnerId == ownerId, cancellationToken);
        
        var firm = firms.FirstOrDefault();

        if (firm == null)
        {
            return false;
        }

        var planExist = await _subscriptionRepository.AnyAsync(p => p.Id == request.PlanId, cancellationToken);

        if (!planExist)
        {
            return false;
        }
        
        firm.SubscriptionPlanId = request.PlanId;
        firm.SubscriptionEndsAt = DateTime.UtcNow.AddDays(30);
        
        await _firmRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
    
}