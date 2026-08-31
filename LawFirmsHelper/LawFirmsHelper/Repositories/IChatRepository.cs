using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    public Task<List<Chat>> GetByLeadAsync(Guid leadId, CancellationToken cancellationToken);
    
    public Task<Chat> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}