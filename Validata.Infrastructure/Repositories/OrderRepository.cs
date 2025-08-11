using Microsoft.EntityFrameworkCore;
using Validata.Domain.Entities;
using Validata.Domain.Repositories;
using Validata.Infrastructure.Persistence;

namespace Validata.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext db) : base(db) { }

    public IQueryable<Order> GetByCustomerOrderedByDate(Guid customerId) =>
        _set.Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.OrderDate);

    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)        // loads Product for each item
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}
