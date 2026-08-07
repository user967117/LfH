namespace LawFirmsHelper.Requests;

public class MessageResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    
    public DateTime CreatedAt { get; set; }
}