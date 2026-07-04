namespace LawFirmsHelper.Models;

public class Agent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Guid FirmId { get; set; }
}