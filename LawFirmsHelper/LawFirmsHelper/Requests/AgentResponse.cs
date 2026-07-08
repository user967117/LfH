using LawFirmsHelper.Models;
using LawFirmsHelper.Services;

namespace LawFirmsHelper.Requests 
{
    public class AgentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AgentStatus Status { get; set; }
        public Guid FirmId { get; set; }
    }
}