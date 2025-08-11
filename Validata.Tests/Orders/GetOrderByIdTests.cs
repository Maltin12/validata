using FluentAssertions;
using Validata.Application.Orders.Commands;
using Validata.Application.Orders.Queries;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Orders;

public class GetOrderByIdTests
{
    [Test]
    public async Task Returns_dto_with_items()
    {
        using var db = TestDb.Create();
        var custRepo = new CustomerRepository(db);
        var orderRepo = new OrderRepository(db);
        var prodRepo = new ProductRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var cid = await new Validata.Application.Customers.Commands.CreateCustomerHandler(custRepo, uow)
            .Handle(new("Liz", "Young", "X3", "44000"), default);

        var p = db.Products.First();
        var oid = await new CreateOrderHandler(orderRepo, prodRepo,custRepo, uow)
            .Handle(new(cid, new() { new(p.Id, 2) }), default);

        var dto = await new GetOrderByIdHandler(orderRepo).Handle(new(oid), default);

        dto.Should().NotBeNull();
        dto!.Items.Should().HaveCount(1);
        dto.TotalPrice.Should().Be(p.Price * 2);
        dto.Items[0].ProductId.Should().Be(p.Id);
    }
}
