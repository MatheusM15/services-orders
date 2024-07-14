using APPLICATION.builder;
using APPLICATION.Fixture;
using DOMAIN.Entities;
using Microsoft.Extensions.DependencyInjection;
using API.Configuration;
using MediatR;
using APPLICATION.Order.GetByIdAsync;
using INFRA.Repositories;
using FluentAssertions;

namespace APPLICATION.Order;

[Collection("DatabaseFixture")]
public class GetByIdTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly OrderBuilder _orderBuilder;
    private readonly OrderProductBuilder _orderProductBuilder;
    private readonly OrderProductIngredientBuilder _orderProductIngredientBuilder;

    private readonly ServiceProvider _serviceProvider;
    private readonly DatabaseFixture _databaseFixture;
    private readonly IMediator _mediator;

    private readonly Guid orderId = Guid.NewGuid();

    public GetByIdTest(DatabaseFixture databaseFixture)
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

    public void Dispose()
    {
        DatabaseFixture.RemoveOrders(_databaseFixture.Context);
    }

    [Fact]
    public async Task Should_Return_The_Order_With_The_Given_Id()
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(orderId));
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
    }

    [Fact]
    public async Task Should_Return_Null_When_Not_Found_Order()
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(Guid.NewGuid()));
        result.Should().BeNull();
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
                    .SetStatus(DOMAIN.Enums.OrderStatus.Received)
                    .SetCustomerId(Guid.NewGuid())
                    .SetIsPaid(false)
                    .SetDiscount(0)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();
    }
}
