using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<Firm> _firmRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IRepository<Plan> _planRepository;
    
    public SubscriptionService(IRepository<Firm> firmRepository, IRepository<Subscription> subscriptionRepository, IRepository<Plan> planRepository)
    {
        _firmRepository = firmRepository;
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
    }
    
    public async Task<bool> AddOrUpdateAsync(AddOrUpdateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var planExists = await _planRepository.GetWhereAsync(p => p.Id == command.PlanId, cancellationToken);

        var firms = await _firmRepository.GetWhereAsync(f => f.Id == command.FirmId && f.OwnerId == command.OwnerId, cancellationToken);
        var firm = firms.FirstOrDefault();
        if (firm == null) return false;
        
        var subscriptions = await _subscriptionRepository.GetWhereAsync(s => s.FirmId == command.FirmId, cancellationToken);
        var currentSubscription = subscriptions.FirstOrDefault();
        
        if (currentSubscription?.PlanId == command.PlanId)
        {
                return false;
        }
        if (currentSubscription != null)
        {
            currentSubscription.PlanId = command.PlanId;
            currentSubscription.CreatedAt = DateTime.UtcNow;
            currentSubscription.ExpiresAt = DateTime.UtcNow.AddDays(30);
            currentSubscription.Status = SubscriptionStatus.Active;
        }

        else
        {
            var newSubscription = new Subscription
            {
                FirmId = command.FirmId,
                PlanId = command.PlanId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                Status = SubscriptionStatus.Active
            };
            await _subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        }
        
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}