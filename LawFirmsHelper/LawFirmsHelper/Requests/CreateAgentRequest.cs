namespace LawFirmsHelper.Requests;

public class CreateAgentRequest
{
    public string Name { get; set; }
    public Guid FirmId { get; set; }
}