using MediatR;
using Microsoft.EntityFrameworkCore;
using Validata.Domain.Abstractions;
using Validata.Domain.Repositories;

namespace Validata.Application.Orders.Commands;

public record UpdateOrderItem(Guid ProductId, int Quantity);
public record UpdateOrderCommand(Guid OrderId, List<UpdateOrderItem> Items) : IRequest<bool>;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IUnitOfWork _uow;

    public UpdateOrderHandler(IOrderRepository orders, IProductRepository products, IUnitOfWork uow)
    { _orders = orders; _products = products; _uow = uow; }

    public async Task<bool> Handle(UpdateOrderCommand r, CancellationToken ct)
    {
        var order = await _orders.GetByIdWithItemsAsync(r.OrderId, ct);
        if (order is null) return false;

        var ids = r.Items.Select(i => i.ProductId).ToList();
        var products = await _products.Query(p => ids.Contains(p.Id)).ToDictionaryAsync(p => p.Id, ct);
        if (products.Count != ids.Count) throw new InvalidOperationException("One or more products not found.");

        order.Items.Clear();
        foreach (var i in r.Items)
        {
            var p = products[i.ProductId];
            order.Items.Add(new Domain.Entities.OrderItem
            {
                ProductId = p.Id,
                Quantity = i.Quantity,
                UnitPrice = p.Price
            });
        }

        order.TotalPrice = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
