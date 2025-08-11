using Validata.Domain.Entities;
using Validata.Domain.Repositories;
using Validata.Infrastructure.Persistence;

namespace Validata.Infrastructure.Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext db) : base(db) { }
}
