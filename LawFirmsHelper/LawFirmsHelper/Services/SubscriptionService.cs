using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<SubscriptionPlan> _planRepository;
    private readonly IRepository<Firm> _firmRepository;
    private readonly IRepository<FirmSubscription> _subscriptionRepository;
    
    
    public SubscriptionService(IRepository<SubscriptionPlan> planRepository, IRepository<Firm> firmRepository, IRepository<FirmSubscription> subscriptionRepository)
    {
        _planRepository = planRepository;
        _firmRepository = firmRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<bool> UpdateSubscriptionAsync(UpdateSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var planExists = await _planRepository.AnyAsync(p => p.Id == request.PlanId, cancellationToken);
        if (!planExists) return false;

        var firms = await _firmRepository.GetWhereAsync(f => f.Id == request.FirmId && f.OwnerId == request.OwnerId, cancellationToken);
        var firm = firms.FirstOrDefault();
        if (firm == null) return false;
        
        var newSubscription = new FirmSubscription
        {
            FirmId = request.FirmId,
            SubscriptionPlanId = request.PlanId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsActive = true
        };
        
        await _subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}