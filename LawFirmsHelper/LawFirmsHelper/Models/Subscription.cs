using LawFirmsHelper.Services;

namespace LawFirmsHelper.Models;

public class Subscription
{
    public Guid Id { get; set; }
    
    public Guid FirmId { get; set; }
    public Firm Firm { get; set; }

    public PlanType Plan { get; set; }
    public SubscriptionStatus Status { get; set; } =  SubscriptionStatus.Active;
    
    public int MaxLeads { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}