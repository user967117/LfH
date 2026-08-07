namespace LawFirmsHelper.Requests;

public class ChatResponse
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
}