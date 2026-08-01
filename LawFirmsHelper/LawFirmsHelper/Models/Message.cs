namespace LawFirmsHelper.Models;

public class Message
{
    public Guid Id { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid LeadId { get; set; }
    public Lead Lead { get; set; }
}