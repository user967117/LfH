using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;

namespace LawFirmsHelper.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<Firm> _firmRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    
    
    public SubscriptionService(IRepository<Firm> firmRepository, IRepository<Subscription> subscriptionRepository)
    {
        _firmRepository = firmRepository;
        _subscriptionRepository = subscriptionRepository;
    }
    
    public async Task<bool> AddSubscriptionAsync(UpdateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(PlanType), command.PlanId))
        {
            return false;
        }

        var firms = await _firmRepository.GetWhereAsync(f => f.Id == command.FirmId && f.OwnerId == command.OwnerId, cancellationToken);
        var firm = firms.FirstOrDefault();
        if (firm == null) return false;
        
        var activeSubscriptions = await _subscriptionRepository.GetWhereAsync(s => s.FirmId == firm.Id && s.Status == SubscriptionStatus.Active, cancellationToken);
        var currentSubscription = activeSubscriptions.FirstOrDefault();

        if (currentSubscription != null)
        {
            if (currentSubscription.Plan == (PlanType)command.PlanId)
            {
                return false;
            }
            
            currentSubscription.Status = SubscriptionStatus.Canceled;
        }
        
        var newSubscription = new Subscription
        {
            FirmId = command.FirmId,
            Plan = (PlanType)command.PlanId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            Status = SubscriptionStatus.Active
        };
        
        await _subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}