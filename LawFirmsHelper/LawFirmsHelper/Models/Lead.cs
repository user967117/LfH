using LawFirmsHelper.Services;

namespace LawFirmsHelper.Models;

public class Lead : IAuditableEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Description { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.New;
    
    public Guid FirmId { get; set; }
    public Firm Firm { get; set; } 
    
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();
}