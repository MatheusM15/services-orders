using API.Configuration;
using APPLICATION.builder;
using APPLICATION.Fixture;
using APPLICATION.Order.GetOrder;
using DOMAIN.Entities;
using DOMAIN.Enums;
using FluentAssertions;
using INFRA.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.Order;

[Collection("DatabaseFixture")]
public class GetOrderQueryTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly OrderBuilder _orderBuilder;
    private readonly OrderProductBuilder _orderProductBuilder;
    private readonly OrderProductIngredientBuilder _orderProductIngredientBuilder;

    private readonly ServiceProvider _serviceProvider;
    private readonly DatabaseFixture _databaseFixture;
    private readonly IMediator _mediator;

    private readonly Guid orderId = Guid.NewGuid();
    private readonly Guid customerId = Guid.NewGuid();
    private readonly bool isPaid = false;
    private readonly double discount = 15.5;
    private readonly OrderStatus status = OrderStatus.Received;

    public GetOrderQueryTest(DatabaseFixture databaseFixture)
    {
        var services = new ServiceCollection();
        _databaseFixture = databaseFixture;

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton(databaseFixture.Context);

        services.AddInjectMediator();
        _orderBuilder = new OrderBuilder();
        _orderProductBuilder = new OrderProductBuilder();
        _orderProductIngredientBuilder = new OrderProductIngredientBuilder();
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        DatabaseFixture.CreateOrder(GetOrders(), databaseFixture);
    }

    [Fact]
    public async Task Should_Return_List_Of_Order()
    {
        var result = await _mediator.Send(new GetOrderQuery());
        result.Should().NotBeNull();
        result.First().Id.Should().Be(orderId);
        result.First().Discount.Should().Be(discount);
        result.First().CustomerId.Should().Be(customerId);
        result.First().IsPaid.Should().Be(isPaid);
        result.First().Status.Should().Be(status);
        result.Count().Should().Be(1);
    }

    public void Dispose()
    {
        DatabaseFixture.RemoveOrders(_databaseFixture.Context);
    }

    public IEnumerable<DOMAIN.Order> GetOrders()
    {
        var ingredients = _orderProductIngredientBuilder
            .SetQuantity(0)
        .SetId(Guid.NewGuid())
            .Build();

        var products = _orderProductBuilder
            .SetQuantity(1)
            .SetIngredients(new List<DOMAIN.Entities.OrderProductIngredient> { ingredients })
            .SetProductId(Guid.NewGuid())
        .Build();

        yield return _orderBuilder
                    .SetId(orderId)
                    .SetStatus(status)
                    .SetCustomerId(customerId)
                    .SetIsPaid(isPaid)
                    .SetDiscount(discount)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();

        yield return _orderBuilder
                    .SetId(Guid.NewGuid())
                    .SetStatus(DOMAIN.Enums.OrderStatus.Finished)
                    .SetCustomerId(customerId)
                    .SetIsPaid(false)
                    .SetDiscount(0)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();
    }

}
