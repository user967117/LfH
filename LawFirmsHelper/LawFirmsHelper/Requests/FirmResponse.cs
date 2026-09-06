namespace LawFirmsHelper.Requests;

public class FirmResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    
}