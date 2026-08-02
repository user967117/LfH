using LawFirmsHelper.Models;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;

namespace LawFirmsHelper.Extentions;

public static class AgentMappingesponse
{
    public static AgentResponse ToResponse(this Agent agent)
    {
        if (agent == null) return null;
        
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
        if (agents == null) return new List<AgentResponse>();
        
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

    public static LeadResponse ToResponse(this Lead lead)
    {
        if (lead == null) return null;

        return new LeadResponse
        {
            Id = lead.Id,
            Name = lead.Name,
            Phone = lead.Phone,
            Email = lead.Email,
            Description = lead.Description,
            Status = lead.Status,
            FirmId = lead.FirmId,
            CreatedAt = lead.CreatedAt,
        };
    }

    public static List<LeadResponse> ToResponse(this IEnumerable<Lead> leads)
    {
        if (leads == null) return new List<LeadResponse>();

        return leads.Select(l => l.ToResponse()).ToList();
    }

    public static Lead ToLead(this CreateLeadRequest request)
    {
        if (request == null) return null;

        return new Lead
        {
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            Description = request.Description,
            FirmId = request.FirmId,
            Status = LeadStatus.New,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Message ToMessage(this CreateMessageRequest request)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static MessageResponse ToResponse(this Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            Text = message.Text,
            CreatedAt = message.CreatedAt,
        };
    }

    public static Chat ToChat(this CreateChatRequest request)
    {
        return new Chat
        {
            Id = Guid.NewGuid(),
            LeadId = request.LeadId,
            Title = request.Title,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static ChatResponse ToResponse(this Chat chat)
    {
        return new ChatResponse
        {
            Id = chat.Id,
            LeadId = chat.LeadId,
            Title = chat.Title,
            CreatedAt = chat.CreatedAt,
        };
    }
}