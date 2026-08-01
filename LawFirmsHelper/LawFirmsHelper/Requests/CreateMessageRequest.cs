namespace LawFirmsHelper.Requests;

public class CreateMessageRequest
{
    public Guid LeadId { get; set; }
    public string Text { get; set; }
}