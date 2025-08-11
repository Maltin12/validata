using Validata.Domain.Entities;

namespace Validata.Domain.Repositories;
public interface IOrderRepository : IRepository<Order>
{
    IQueryable<Order> GetByCustomerOrderedByDate(Guid customerId);
    Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct);

}
