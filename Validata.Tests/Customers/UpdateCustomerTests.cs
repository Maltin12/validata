using FluentAssertions;
using Validata.Application.Customers.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Customers;

public class UpdateCustomerTests
{
    [Test]
    public async Task Updates_existing_customer()
    {
        using var db = TestDb.Create();
        var repo = new CustomerRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var create = new CreateCustomerHandler(repo, uow);
        var id = await create.Handle(new("Bob", "Smith", "A1", "9000"), default);

        var update = new UpdateCustomerHandler(repo, uow);
        var ok = await update.Handle(new(id, "Bobby", "Smith", "A2", "9000"), default);

        ok.Should().BeTrue();
        db.Customers.Single(x => x.Id == id).Address.Should().Be("A2");
    }

    [Test]
    public async Task Update_returns_false_when_customer_not_found()
    {
        using var db = TestDb.Create();
        var repo = new CustomerRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var ok = await new UpdateCustomerHandler(repo, uow)
            .Handle(new(Guid.NewGuid(), "X", "Y", "Z", "1"), default);

        ok.Should().BeFalse();
    }
}
