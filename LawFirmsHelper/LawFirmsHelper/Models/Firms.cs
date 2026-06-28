namespace LawFirmsHelper.Models;

public class Firms
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OwnerID { get; set; }
    public DateTime CreatedAt { get; set; }
}