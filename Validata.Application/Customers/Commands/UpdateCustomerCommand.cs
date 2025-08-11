using MediatR;
using Validata.Domain.Repositories;
using Validata.Domain.Abstractions;

namespace Validata.Application.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, string FirstName, string LastName, string Address, string PostalCode) : IRequest<bool>;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCustomerHandler(ICustomerRepository repo, IUnitOfWork uow)
    {
        _repo = repo; _uow = uow;
    }

    public async Task<bool> Handle(UpdateCustomerCommand r, CancellationToken ct)
    {
        var c = await _repo.GetByIdAsync(r.Id);
        if (c is null) return false;

        c.FirstName = r.FirstName; c.LastName = r.LastName; c.Address = r.Address; c.PostalCode = r.PostalCode;
        _repo.Update(c);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
