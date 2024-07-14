using API.Configuration;
using APPLICATION.builder;
using APPLICATION.Fixture;
using APPLICATION.Order.CreateOrder;
using DOMAIN.Entities;
using DOMAIN.Enums;
using FluentAssertions;
using INFRA.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION.Order;

[Collection("DatabaseFixture")]
public class CreateOrderTest : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly OrderProductBuilder _orderProductBuilder;
    private readonly OrderProductIngredientBuilder _orderProductIngredientBuilder;

    private readonly ServiceProvider _serviceProvider;
    private readonly DatabaseFixture _databaseFixture;
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orderRepository;

    private readonly Guid _customerId = Guid.NewGuid();
    private readonly double _discount = 200;

    public CreateOrderTest(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _orderProductBuilder = new OrderProductBuilder();
        _orderProductIngredientBuilder = new OrderProductIngredientBuilder();

        var services = new ServiceCollection();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton(databaseFixture.Context);

        services.AddInjectMediator();
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
        _orderRepository = _serviceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    public async Task Should_Create_Order()
    {
        var command = CreateCommand();

        var result = await _mediator.Send(command);

        var order = await _orderRepository.GetOrderByIdAsync(result.Id, d => CreateOrderDtoTest.ToEntity(d));

        order.Should().NotBeNull();
        order.CustomerId.Should().Be(_customerId);
        order.Discount.Should().Be(_discount);
        order.Status.Should().Be(OrderStatus.Received);
        order.IsPaid.Should().BeFalse();
    }

    public void Dispose()
    {
        DatabaseFixture.RemoveOrders(_databaseFixture.Context);
    }

    private CreateOrderCommand CreateCommand()
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

        var command = new CreateOrderCommand(_customerId, _discount, new List<OrderProduct> { products });

        return command;
    }

    public class CreateOrderDtoTest
    {
        public Guid Id { get; set; }
        public double Discount { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsPaid { get; set; }

        public static CreateOrderDtoTest ToEntity(DOMAIN.Order order)
            => new CreateOrderDtoTest
            {
                Id = order.Id,
                Discount = order.Discount,
                CustomerId = order.CustomerId,
                Status = order.Status,
                IsPaid = order.IsPaid,
            };
    }
}
