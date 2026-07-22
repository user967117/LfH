using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IFirmRepository _firmRepository;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IRepository<Plan> _planRepository;
    
    public SubscriptionService(IFirmRepository firmRepository, IRepository<Subscription> subscriptionRepository, IRepository<Plan> planRepository)
    {
        _firmRepository = firmRepository;
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
    }
    
    public async Task<bool> AddOrUpdateAsync(AddOrUpdateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var firm = await _firmRepository.GetFirmWithSubscriptionAsync(command.FirmId, command.OwnerId, cancellationToken);
            
        if (firm == null) return false;
        
        var currentSubscription = firm.Subscription;
        
        if (currentSubscription?.PlanId == command.PlanId)
        {
                return false;
        }
        if (currentSubscription != null)
        {
            currentSubscription.PlanId = command.PlanId;
            currentSubscription.CreatedAt = DateTime.UtcNow;
            currentSubscription.ExpiresAt = DateTime.UtcNow.AddMonths(1);
            currentSubscription.Status = SubscriptionStatus.Active;
        }

        else
        {
            var newSubscription = new Subscription
            {
                FirmId = command.FirmId,
                PlanId = command.PlanId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(1),
                Status = SubscriptionStatus.Active
            };
            await _subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        }
        
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}