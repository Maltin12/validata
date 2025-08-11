using FluentAssertions;
using Validata.Application.Orders.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Orders;

public class DeleteOrderTests
{
    [Test]
    public async Task Deletes_order_only_not_customer()
    {
        using var db = TestDb.Create();
        var custRepo = new CustomerRepository(db);
        var orderRepo = new OrderRepository(db);
        var prodRepo = new ProductRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var cid = await new Validata.Application.Customers.Commands.CreateCustomerHandler(custRepo, uow)
            .Handle(new("Mia", "Fox", "T1", "33333"), default);

        var pid = db.Products.First().Id;
        var oid = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
            .Handle(new(cid, new() { new(pid, 1) }), default);

        (await new DeleteOrderHandler(orderRepo, uow).Handle(new(oid), default))
            .Should().BeTrue();

        db.Orders.Any(o => o.Id == oid).Should().BeFalse();
        db.Customers.Any(c => c.Id == cid).Should().BeTrue();
    }

    [Test]
    public async Task Delete_returns_false_when_order_missing()
    {
        using var db = TestDb.Create();
        var orderRepo = new OrderRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var ok = await new DeleteOrderHandler(orderRepo, uow)
            .Handle(new(Guid.NewGuid()), default);

        ok.Should().BeFalse();
    }
}
