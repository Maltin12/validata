using MediatR;
using Microsoft.EntityFrameworkCore;
using Validata.Application.Common.DTOs;
using Validata.Domain.Repositories;

namespace Validata.Application.Customers.Queries;

public record GetCustomerOrdersByDateQuery(Guid CustomerId) : IRequest<List<OrderOutDto>>;

public class GetCustomerOrdersByDateHandler : IRequestHandler<GetCustomerOrdersByDateQuery, List<OrderOutDto>>
{
    private readonly IOrderRepository _orders;

    public GetCustomerOrdersByDateHandler(IOrderRepository orders) => _orders = orders;

    public async Task<List<OrderOutDto>> Handle(GetCustomerOrdersByDateQuery r, CancellationToken ct)
    {
        var q = _orders.GetByCustomerOrderedByDate(r.CustomerId);

        return await q.Select(o => new OrderOutDto(
                o.Id,
                o.OrderDate,
                o.TotalPrice,
                o.Items.Select(i => new OrderItemOutDto(i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice)).ToList()
            )).ToListAsync(ct);
    }
}
