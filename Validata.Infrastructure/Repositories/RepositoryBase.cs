using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Validata.Domain.Repositories;
using Validata.Infrastructure.Persistence;

namespace Validata.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _db;
    protected readonly DbSet<T> _set;

    public RepositoryBase(AppDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public Task<T?> GetByIdAsync(Guid id) => _set.FindAsync(id).AsTask();
    public Task AddAsync(T entity) => _set.AddAsync(entity).AsTask();
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);

    public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
        => predicate is null ? _set.AsQueryable() : _set.Where(predicate);
}
