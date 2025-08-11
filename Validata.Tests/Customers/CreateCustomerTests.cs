using FluentAssertions;
using NUnit.Framework.Internal;
using Validata.Application.Customers.Commands;
using Validata.Domain.Abstractions;
using Validata.Infrastructure;
using Validata.Infrastructure.Repositories;
using Validata.Tests.Helpers;

namespace Validata.Tests.Customers;

public class CreateCustomerTests
{
    [Test]
    public async Task Creates_customer_and_persists()
    {
        using var db = TestDb.Create();
        var repo = new CustomerRepository(db);
        IUnitOfWork uow = new UnitOfWork(db);

        var handler = new CreateCustomerHandler(repo, uow);

        var id = await handler.Handle(new("Alice", "Jones", "Main 10", "10000"), default);

        db.Customers.Single(c => c.Id == id).FirstName.Should().Be("Alice");
    }
}
