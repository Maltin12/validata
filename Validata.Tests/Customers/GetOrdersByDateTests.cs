using FluentAssertions;
using Validata.Application.Customers.Commands;
using Validata.Application.Customers.Queries;
using Validata.Application.Orders.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Customers
{
    public class GetOrdersByDateTests
    {
        [Test]
        public async Task Returns_orders_sorted_by_date_for_customer()
        {
            using var db = TestDb.Create();

            var custRepo = new CustomerRepository(db);
            var orderRepo = new OrderRepository(db);
            var prodRepo = new ProductRepository(db);
            IUnitOfWork uow = new UnitOfWork(db);

            // create customer
            var cid = await new CreateCustomerHandler(custRepo, uow)
                .Handle(new("Ana", "Bell", "S1", "20000"), default);

            // pick two products
            var p1 = db.Products.First().Id;
            var p2 = db.Products.Skip(1).First().Id;

            // create 2 orders (NOTE: pass custRepo too)
            var o1 = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
                .Handle(new(cid, new() { new(p1, 1) }), default);

            var o2 = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
                .Handle(new(cid, new() { new(p2, 2) }), default);

            // make dates deterministic and in ascending order
            db.Orders.Single(o => o.Id == o1).OrderDate = DateTime.UtcNow.AddDays(-2);
            db.Orders.Single(o => o.Id == o2).OrderDate = DateTime.UtcNow.AddDays(-1);
            await db.SaveChangesAsync();

            // query
            var list = await new GetCustomerOrdersByDateHandler(orderRepo)
                .Handle(new(cid), default);

            list.Select(x => x.Id).Should().ContainInOrder(o1, o2);
        }
    }
}
