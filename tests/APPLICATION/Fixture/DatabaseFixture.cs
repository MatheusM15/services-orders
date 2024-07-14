using INFRA.Context;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.Fixture;

public class DatabaseFixture : IDisposable
{
    public OrderContext Context { get; private set; }

    public DatabaseFixture()
    {

        var options = new DbContextOptionsBuilder<OrderContext>()
      .UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=123;Database=postgres_order_teste")
      .Options;

        Context = new OrderContext(options);

        Context.Database.Migrate();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }

    public static void CreateOrder(IEnumerable<DOMAIN.Order> orders, DatabaseFixture fixture)
    {
        fixture.Context.AddRange(orders);
        fixture.Context.SaveChanges();
    }
    public static void RemoveOrders(OrderContext context)
    {
        var orders = context.Orders;
        context.RemoveRange(orders);
        context.SaveChanges();
    }

}
