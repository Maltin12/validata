using Microsoft.EntityFrameworkCore;
using Validata.Domain.Entities;

namespace Validata.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(b =>
        {
            b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            b.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            b.Property(p => p.Address).IsRequired().HasMaxLength(200);
            b.Property(p => p.PostalCode).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Product>(b =>
        {
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Price).HasColumnType("decimal(18,2)");

            b.HasData(
                new Product { Id = new Guid("f36ad4d2-3c89-43f2-921d-c552d04206de"), Name = "Dell XPS 13 Laptop", Price = 1200.00m },
                new Product { Id = new Guid("171e7e33-fe1e-4240-bfdf-802aafe0ffb8"), Name = "Apple iPhone 15 Pro", Price = 999.99m },
                new Product { Id = new Guid("c1fffd65-3a2b-4670-9998-9cdf97fd3dd6"), Name = "Samsung 55\" 4K Smart TV", Price = 650.00m },
                new Product { Id = new Guid("afb87ec7-9290-4a0f-bcb8-0750cc144e11"), Name = "Sony WH-1000XM5 Headphones", Price = 349.99m },
                new Product { Id = new Guid("f95f8f03-ddfd-4a6b-9b9b-1c523f6ba17d"), Name = "Logitech MX Master 3S", Price = 99.99m }
            );
        });

        modelBuilder.Entity<Order>(b =>
        {
            // Configure TotalPrice
            b.Property(p => p.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Relationship: Order -> Customer
            b.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDate is required
            b.Property(p => p.OrderDate)
                .IsRequired();

            // Index on CustomerId
            b.HasIndex(o => o.CustomerId);

            // Composite index on CustomerId + OrderDate
            b.HasIndex(o => new { o.CustomerId, o.OrderDate });
        });


        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(i => i.Id);

            b.Property(i => i.Id).ValueGeneratedNever();

            b.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");

            b.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(i => i.Product)
                .WithMany()                   
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(i => i.OrderId);
            b.HasIndex(i => i.ProductId);
        });


    }
}
