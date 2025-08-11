using MediatR;
using Validata.Application.Common.DTOs;
using Validata.Domain.Repositories;

namespace Validata.Application.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _repo;

    public GetCustomerByIdHandler(ICustomerRepository repo) => _repo = repo;

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery r, CancellationToken ct)
    {
        var c = await _repo.GetByIdAsync(r.Id);
        return c is null ? null : new CustomerDto(c.Id, c.FirstName, c.LastName, c.Address, c.PostalCode);
    }
}
