using DOMAIN;
using DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Context;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<OrderProductIngredient> OrderProductIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(o => o.Number)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderProduct>()
            .HasMany(o => o.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

}
