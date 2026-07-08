using System.Linq.Expressions;

namespace LawFirmsHelper.Repositories;

public interface IRepository<T> where T : class
{
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    
}