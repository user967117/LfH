using LawFirmsHelper.Models;
using LawFirmsHelper.Services;

namespace LawFirmsHelper.Requests;

public class ChatMessageResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid ActorId { get; set; }
    public string ActorName { get; set; }
    public ActorType ActorType { get; set; }
}