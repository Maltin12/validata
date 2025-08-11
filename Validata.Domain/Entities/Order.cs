namespace Validata.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
