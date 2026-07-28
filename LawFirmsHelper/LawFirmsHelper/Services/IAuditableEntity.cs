namespace LawFirmsHelper.Services;

public class IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}