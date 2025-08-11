using MediatR;
using Validata.Application.Common.DTOs;
using Validata.Domain.Repositories;

namespace Validata.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderOutDto?>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderOutDto?>
{
    private readonly IOrderRepository _orders;
    public GetOrderByIdHandler(IOrderRepository orders) => _orders = orders;

    public async Task<OrderOutDto?> Handle(GetOrderByIdQuery r, CancellationToken ct)
    {
        var o = await _orders.GetByIdWithItemsAsync(r.Id, ct);
        if (o is null) return null;

        return new OrderOutDto(
            o.Id, o.OrderDate, o.TotalPrice,
            o.Items.Select(i => new OrderItemOutDto(i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice)).ToList()
        );
    }
}
