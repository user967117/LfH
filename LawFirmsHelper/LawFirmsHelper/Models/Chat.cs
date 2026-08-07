namespace LawFirmsHelper.Models;

public class Chat
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid LeadId { get; set; }
    public Lead? Lead { get; set; }
    
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}