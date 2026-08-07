namespace LawFirmsHelper.Requests;

public class CreateChatRequest
{
    public Guid LeadId { get; set; }
    
    public string Title { get; set; }
}