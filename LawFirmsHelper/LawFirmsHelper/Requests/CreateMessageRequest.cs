namespace LawFirmsHelper.Requests;

public class CreateMessageRequest
{
    public Guid ChatId { get; set; }
    public string Text { get; set; }
}