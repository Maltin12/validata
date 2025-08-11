using FluentAssertions;
using Validata.Application.Customers.Commands;
using Validata.Application.Orders.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Customers;

public class DeleteCustomerTests
{
    [Test]
    public async Task Deletes_customer_and_cascades_orders()
    {
        using var db = TestDb.Create();

        var custRepo = new CustomerRepository(db);
        var orderRepo = new OrderRepository(db);
        var prodRepo = new ProductRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var cid = await new CreateCustomerHandler(custRepo, uow)
            .Handle(new("John", "Doe", "X", "11111"), default);

        var pid = db.Products.First().Id;

        // ⬇️ pass custRepo too
        var oid = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
            .Handle(new(cid, new() { new(pid, 3) }), default);

        var deleted = await new DeleteCustomerHandler(custRepo, uow)
            .Handle(new(cid), default);

        deleted.Should().BeTrue();
        db.Customers.Any(c => c.Id == cid).Should().BeFalse();
        db.Orders.Any(o => o.Id == oid).Should().BeFalse(); // cascaded
    }
}
