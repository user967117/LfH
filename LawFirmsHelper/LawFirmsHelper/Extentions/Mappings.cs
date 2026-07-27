using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;

namespace LawFirmsHelper.Extentions;

public static class AgentMappingesponse
{
    public static AgentResponse ToResponse(this Agent agent)
    {
        if (agent == null)
        {
            return null;
        }

        return new AgentResponse
        {
            Id = agent.Id,
            Name = agent.Name,
            Status = agent.Status,
            FirmId = agent.FirmId
        };
    }

    public static List<AgentResponse> ToResponse(this IEnumerable<Agent> agents)
    {
        if (agents == null)
        {
            return new List<AgentResponse>();
        }
        return agents.Select(a => a.ToResponse()).ToList();
    }

    public static Subscription ToSubscription(this AddOrUpdateSubscriptionCommand command)
    {
        return new Subscription
        {
            FirmId = command.FirmId,
            PlanId = command.PlanId,
            ExpiresAt = DateTime.UtcNow.AddMonths(1),
            Status = SubscriptionStatus.Active
        };
    }
}