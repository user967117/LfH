namespace LawFirmsHelper.Models;

public class Currency
{
    public int Id { get; set; }
    public string Code { get; set; }
    
    public string Symbol { get; set; }

    public ICollection<Plan> Plans { get; set; } = new List<Plan>();
}