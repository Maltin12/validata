using MediatR;
using Validata.Domain.Abstractions;
using Validata.Domain.Repositories;

namespace Validata.Application.Orders.Commands;

public record DeleteOrderCommand(Guid OrderId) : IRequest<bool>;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public DeleteOrderHandler(IOrderRepository orders, IUnitOfWork uow)
    { _orders = orders; _uow = uow; }

    public async Task<bool> Handle(DeleteOrderCommand r, CancellationToken ct)
    {
        var order = await _orders.GetByIdWithItemsAsync(r.OrderId, ct);
        if (order is null) return false;

        _orders.Remove(order);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
