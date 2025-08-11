namespace Validata.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PostalCode { get; set; } = default!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
