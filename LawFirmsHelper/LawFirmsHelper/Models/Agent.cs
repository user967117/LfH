using LawFirmsHelper.Services;

namespace LawFirmsHelper.Models;

public class Agent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public AgentStatus Status { get; set; } =  AgentStatus.Active;
    public Guid FirmId { get; set; }
}