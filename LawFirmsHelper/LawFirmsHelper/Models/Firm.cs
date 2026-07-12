using Microsoft.AspNetCore.Identity;

namespace LawFirmsHelper.Models;

public class Firm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    
    public IdentityUser Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int? SubscriptionPlanId { get; set; }
    public SubscriptionPlan? SubscriptionPlan { get; set; }
    public DateTime SubscriptionEndsAt { get; set; }
    
    public ICollection<Agent> Agents { get; set; } = new List<Agent>();
}