namespace LawFirmsHelper.Models;

public class Plan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MaxLeads { get; set; }
    
    public int CurrencyId { get; set; }
    public Currency Currency { get; set; } = null;
}