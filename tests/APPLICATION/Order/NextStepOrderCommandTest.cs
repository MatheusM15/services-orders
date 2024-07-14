using API.Configuration;
using APPLICATION.builder;
using APPLICATION.Fixture;
using APPLICATION.Order.NextStepOrder;
using DOMAIN.Entities;
using DOMAIN.Enums;
using FluentAssertions;
using INFRA.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.Order;

[Collection("DatabaseFixture")]
public class NextStepOrderCommandTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly OrderBuilder _orderBuilder;
    private readonly OrderProductBuilder _orderProductBuilder;
    private readonly OrderProductIngredientBuilder _orderProductIngredientBuilder;

    private readonly ServiceProvider _serviceProvider;
    private readonly DatabaseFixture _databaseFixture;
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orderRepository;

    public static Guid firstOrderId = new Guid("ea3a3faf-5386-4827-9f50-229ce72081c5");
    public static Guid secondOrderId = new Guid("d01ef158-0f91-4abd-b4b4-cc0ccea85cca");
    public static Guid thirdOrderId = new Guid("e285f887-3ab0-42f6-9305-a2684905e479");

    public NextStepOrderCommandTest(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _orderBuilder = new OrderBuilder();
        _orderProductBuilder = new OrderProductBuilder();
        _orderProductIngredientBuilder = new OrderProductIngredientBuilder();

        var services = new ServiceCollection();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton(databaseFixture.Context);

        services.AddInjectMediator();
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
        _orderRepository = _serviceProvider.GetRequiredService<IOrderRepository>();

        DatabaseFixture.CreateOrder(GetOrders(), databaseFixture);
    }

    [Theory]
    [InlineData("ea3a3faf-5386-4827-9f50-229ce72081c5", OrderStatus.InProgress)]
    [InlineData("d01ef158-0f91-4abd-b4b4-cc0ccea85cca", OrderStatus.Ready)]
    [InlineData("e285f887-3ab0-42f6-9305-a2684905e479", OrderStatus.Finished)]
    public async Task Should_Set_Order_Status(Guid orderId, OrderStatus status)
    {
        var command = await _mediator.Send(new NextStepOrderCommand(orderId));
        var order = await _orderRepository.GetByIdAsync(orderId);

        command.Status.Should().Be(status);
        order.Status.Should().Be(status);
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
                    .SetId(firstOrderId)
                    .SetStatus(DOMAIN.Enums.OrderStatus.Received)
                    .SetCustomerId(Guid.NewGuid())
                    .SetIsPaid(false)
                    .SetDiscount(0)
                    .AddProduct(new List<OrderProduct> { products })
                    .Build();

        yield return _orderBuilder
               .SetId(secondOrderId)
               .SetStatus(DOMAIN.Enums.OrderStatus.InProgress)
               .SetCustomerId(Guid.NewGuid())
               .SetIsPaid(false)
               .SetDiscount(0)
               .AddProduct(new List<OrderProduct> { products })
               .Build();

        yield return _orderBuilder
              .SetId(thirdOrderId)
              .SetStatus(DOMAIN.Enums.OrderStatus.Ready)
              .SetCustomerId(Guid.NewGuid())
              .SetIsPaid(false)
              .SetDiscount(0)
              .AddProduct(new List<OrderProduct> { products })
              .Build();
    }
}
