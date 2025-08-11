using System.Linq.Expressions;

namespace Validata.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
}
