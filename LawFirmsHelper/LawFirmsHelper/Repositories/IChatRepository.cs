using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    public Task<List<Chat>> GetByIdAsync(Guid leadId, CancellationToken cancellationToken);
}