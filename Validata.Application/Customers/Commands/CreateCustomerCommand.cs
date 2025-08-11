using MediatR;
using Validata.Domain.Entities;
using Validata.Domain.Repositories;
using Validata.Domain.Abstractions;

namespace Validata.Application.Customers.Commands;

public record CreateCustomerCommand(string FirstName, string LastName, string Address, string PostalCode) : IRequest<Guid>;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCustomerHandler(ICustomerRepository repo, IUnitOfWork uow)
    {
        _repo = repo; _uow = uow;
    }

    public async Task<Guid> Handle(CreateCustomerCommand r, CancellationToken ct)
    {
        var c = new Customer { FirstName = r.FirstName, LastName = r.LastName, Address = r.Address, PostalCode = r.PostalCode };
        await _repo.AddAsync(c);
        await _uow.SaveChangesAsync(ct);
        return c.Id;
    }
}
