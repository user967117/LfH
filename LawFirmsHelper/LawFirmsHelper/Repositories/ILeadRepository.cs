using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface ILeadRepository : IRepository<Lead>
{
    Task<Lead> AddAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<int> GetCountAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Lead?> GetByEmailAsync(Guid firmId, string email, CancellationToken cancellationToken = default);
}