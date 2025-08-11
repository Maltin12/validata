using MediatR;
using Validata.Domain.Repositories;
using Validata.Domain.Abstractions;

namespace Validata.Application.Customers.Commands;

public record DeleteCustomerCommand(Guid Id) : IRequest<bool>;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCustomerHandler(ICustomerRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(DeleteCustomerCommand r, CancellationToken ct)
    {
        var c = await _repo.GetByIdAsync(r.Id);
        if (c is null) return false;

        _repo.Remove(c);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
