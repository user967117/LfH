namespace LawFirmsHelper.Requests;

public class CreateLeadRequest
{
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
    
    public Guid FirmId { get; set; }
}