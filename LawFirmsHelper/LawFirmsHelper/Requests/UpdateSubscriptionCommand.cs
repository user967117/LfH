namespace LawFirmsHelper.Services;

public class UpdateSubscriptionCommand
{
    public Guid FirmId { get; set; }
    public string OwnerId { get; set; }
    public int PlanId { get; set; }
}