using DOMAIN.Enums;
using INFRA.Repositories;
using MediatR;

namespace APPLICATION.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {

        var order = DOMAIN.Order.CreateOrder(request.CustomerId, request.Discount, OrderStatus.Received);

        order.AddProducts(request.Product);

        await _orderRepository.AddAsync(order);
        _orderRepository.SaveChangesAsync();

        return CreateOrderResponse.ToResponse(order);
    }
}
