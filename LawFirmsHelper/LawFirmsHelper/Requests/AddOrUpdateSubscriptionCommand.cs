namespace LawFirmsHelper.Services;

public class AddOrUpdateSubscriptionCommand
{
    public Guid FirmId { get; set; }
    public string OwnerId { get; set; }
    public int PlanId { get; set; }
}