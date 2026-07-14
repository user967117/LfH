namespace LawFirmsHelper.Models;

public class FirmSubscription
{
    public Guid Id { get; set; }
    
    public Guid FirmId { get; set; }
    public Firm Firm { get; set; }
    
    public int SubscriptionPlanId { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public bool IsActive { get; set; }
}