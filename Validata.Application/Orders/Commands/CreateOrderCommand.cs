using MediatR;
using Microsoft.EntityFrameworkCore;
using Validata.Domain.Abstractions;
using Validata.Domain.Entities;
using Validata.Domain.Repositories;

namespace Validata.Application.Orders.Commands;

public record CreateOrderItem(Guid ProductId, int Quantity);
public record CreateOrderCommand(Guid CustomerId, List<CreateOrderItem> Items) : IRequest<Guid>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _uow;

    public CreateOrderHandler(IOrderRepository orders, IProductRepository products, ICustomerRepository customers, IUnitOfWork uow)
    { _orders = orders; _products = products; _customers = customers; _uow = uow; }

    public async Task<Guid> Handle(CreateOrderCommand r, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(r.CustomerId);
        if (customer is null) throw new InvalidOperationException("Customer not found.");

        var productIds = r.Items.Select(i => i.ProductId).ToList();
        var prodLookup = await _products.Query(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, ct);

        var order = new Order { CustomerId = r.CustomerId, OrderDate = DateTime.UtcNow };
        foreach (var item in r.Items)
        {
            if (!prodLookup.TryGetValue(item.ProductId, out var product))
                throw new InvalidOperationException($"Product {item.ProductId} not found.");

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalPrice = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        await _orders.AddAsync(order);
        await _uow.SaveChangesAsync(ct);
        return order.Id;
    }
}
