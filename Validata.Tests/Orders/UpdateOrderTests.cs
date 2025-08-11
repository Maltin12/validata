using FluentAssertions;
using Validata.Application.Orders.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Orders;

public class UpdateOrderTests
{
    [Test]
    public async Task Replaces_items_and_recomputes_total()
    {
        using var db = TestDb.Create();
        var custRepo = new CustomerRepository(db);
        var orderRepo = new OrderRepository(db);
        var prodRepo = new ProductRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var cid = await new Validata.Application.Customers.Commands.CreateCustomerHandler(custRepo, uow)
            .Handle(new("Tom", "Ray", "S2", "50000"), default);

        var p1 = db.Products.First();
        var p2 = db.Products.Skip(1).First();

        var oid = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
            .Handle(new(cid, new() { new(p1.Id, 1) }), default);

        var ok = await new UpdateOrderHandler(orderRepo, prodRepo, uow)
            .Handle(new(oid, new() { new(p2.Id, 2) }), default);

        ok.Should().BeTrue();
        var order = db.Orders.Single(o => o.Id == oid);
        order.TotalPrice.Should().Be(p2.Price * 2);
        order.Items.Single().ProductId.Should().Be(p2.Id);
    }

    [Test]
    public async Task Update_returns_false_when_order_not_found()
    {
        using var db = TestDb.Create();
        var orderRepo = new OrderRepository(db);
        var prodRepo = new ProductRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var ok = await new UpdateOrderHandler(orderRepo, prodRepo, uow)
            .Handle(new(Guid.NewGuid(), new() { new(Guid.NewGuid(), 1) }), default);

        ok.Should().BeFalse();
    }
}
