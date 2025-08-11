using FluentAssertions;
using Validata.Application.Customers.Commands;
using Validata.Application.Orders.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Orders
{
    public class CreateOrderTests
    {
        [Test]
        public async Task Creates_order_and_calculates_total()
        {
            using var db = TestDb.Create();

            var custRepo = new CustomerRepository(db);
            var orderRepo = new OrderRepository(db);
            var prodRepo = new ProductRepository(db);
            IUnitOfWork uow = new UnitOfWork(db);

            var cid = await new CreateCustomerHandler(custRepo, uow)
                .Handle(new("Sam", "Rex", "Z1", "10000"), default);

            var product = db.Products.First(); // seeded
            var qty = 3;

            // ⬇️ pass custRepo as 3rd argument
            var oid = await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
                .Handle(new(cid, new() { new(product.Id, qty) }), default);

            var order = db.Orders.Single(o => o.Id == oid);
            order.TotalPrice.Should().Be(product.Price * qty);
            order.Items.Single().UnitPrice.Should().Be(product.Price);
        }

        [Test]
        public async Task Throws_when_product_missing()
        {
            using var db = TestDb.Create();

            var custRepo = new CustomerRepository(db);
            var orderRepo = new OrderRepository(db);
            var prodRepo = new ProductRepository(db);
            IUnitOfWork uow = new UnitOfWork(db);

            var cid = await new CreateCustomerHandler(custRepo, uow)
                .Handle(new("Zed", "Hay", "A4", "10101"), default);

            var missingProductId = Guid.NewGuid();

            var act = async () => await new CreateOrderHandler(orderRepo, prodRepo, custRepo, uow)
                .Handle(new(cid, new() { new(missingProductId, 1) }), default);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
