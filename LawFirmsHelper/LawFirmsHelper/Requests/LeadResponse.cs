using LawFirmsHelper.Services;

namespace LawFirmsHelper.Requests;

public class LeadResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    
    public LeadStatus Status { get; set; }
    
    public Guid FirmId { get; set; }
    public DateTime CreatedAt { get; set; }
}