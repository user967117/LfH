namespace LawFirmsHelper.Requests;

public class UpdateSubscriptionRequest
{
    public int PlanId { get; set; }
    
    public Guid FirmId { get; set; }   
    public string OwnerId { get; set; }
}   