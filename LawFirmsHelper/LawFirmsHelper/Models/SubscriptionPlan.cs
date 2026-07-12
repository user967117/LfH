namespace LawFirmsHelper.Models;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public int MaxLeads { get; set; }
}