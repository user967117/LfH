using LawFirmsHelper.Services;

namespace LawFirmsHelper.Models;

public class Actor
{
    public Guid Id { get; set; }
    public ActorType Type { get; set; }
}