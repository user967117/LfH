using LawFirmsHelper.Models;

namespace LawFirmsHelper.Repositories;

public interface ILeadRepository
{
    Task<Lead> AddAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}