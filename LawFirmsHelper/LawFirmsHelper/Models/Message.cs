namespace LawFirmsHelper.Models;

public class Message
{
    public Guid Id { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; }
    
    public Guid ActorId { get; set; }
    public Actor Actor { get; set; }
}