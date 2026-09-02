namespace LawFirmsHelper.Requests;

public class CreateChatRequest
{
    public Guid FirmId { get; set; }
    public Guid? LeadId { get; set; }
    public string Title { get; set; }
}