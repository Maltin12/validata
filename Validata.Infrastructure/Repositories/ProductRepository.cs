using Validata.Domain.Entities;
using Validata.Domain.Repositories;
using Validata.Infrastructure.Persistence;

namespace Validata.Infrastructure.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(AppDbContext db) : base(db) { }
}
