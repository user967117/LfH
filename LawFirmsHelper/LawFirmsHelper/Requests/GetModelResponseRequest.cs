using LawFirmsHelper.Models;

namespace LawFirmsHelper.Requests;

public class GetModelResponseRequest
{
    public Guid FirmId { get; set; }
    public Guid ChatId { get; set; }
    public List<Message> DbHistory { get; set; } = new();
}