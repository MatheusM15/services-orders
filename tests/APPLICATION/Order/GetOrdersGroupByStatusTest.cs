using API.Configuration;
using APPLICATION.builder;
using APPLICATION.Fixture;
using APPLICATION.Order.GetOrdersGroupByStatus;
using DOMAIN.Entities;
using FluentAssertions;
using INFRA.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.Order;

[Collection("DatabaseFixture")]
public class GetOrdersGroupByStatusTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly OrderBuilder _orderBuilder;
    private readonly OrderProductBuilder _orderProductBuilder;
    private readonly OrderProductIngredientBuilder _orderProductIngredientBuilder;
    private readonly ServiceProvider _serviceProvider;
    private readonly DatabaseFixture _databaseFixture;
    private readonly IMediator _mediator;

    private readonly Guid _firstOrderId = Guid.NewGuid();

    private readonly Guid _secondOrderId = Guid.NewGuid();

    private readonly Guid _thirdOrderId = Guid.NewGuid();

    public GetOrdersGroupByStatusTest(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _orderProductBuilder = new OrderProductBuilder();
        _orderBuilder = new OrderBuilder();
        _orderProductIngredientBuilder = new OrderProductIngredientBuilder();

        var services = new ServiceCollection();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton(databaseFixture.Context);

        services.AddInjectMediator();
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        DatabaseFixture.CreateOrder(GetOrders(), _databaseFixture);
    }

    public void Dispose()
    {
        DatabaseFixture.RemoveOrders(_databaseFixture.Context);
    }

    [Fact]
    public async Task Should_Return_List_Of_Order()
    {
        var result = await _mediator.Send(new GetOrdersGroupByStatusQuery());

        result.InProgress.Count().Should().Be(1);
        result.Ready.Count().Should().Be(1);
        result.Received.Count().Should().Be(1);
        result.InProgress.First().Id.Should().Be(_thirdOrderId);
        result.Ready.First().Id.Should().Be(_secondOrderId);
        result.Received.First().Id.Should().Be(_firstOrderId);
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
                    .SetId(_firstOrderId)
                    .SetStatus(DOMAIN.Enums.OrderStatus.Received)
                    .SetCustomerId(Guid.NewGuid())
                    .SetIsPaid(false)
                    .SetDiscount(0)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();

        yield return _orderBuilder
                    .SetId(_secondOrderId)
                    .SetStatus(DOMAIN.Enums.OrderStatus.Ready)
                    .SetCustomerId(Guid.NewGuid())
                    .SetIsPaid(false)
                    .SetDiscount(0)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();

        yield return _orderBuilder
                   .SetId(_thirdOrderId)
                   .SetStatus(DOMAIN.Enums.OrderStatus.InProgress)
                   .SetCustomerId(Guid.NewGuid())
                   .SetIsPaid(false)
                   .SetDiscount(0)
                   .AddProduct(new List<OrderProduct> { products })
                   .Build();
    }
}
