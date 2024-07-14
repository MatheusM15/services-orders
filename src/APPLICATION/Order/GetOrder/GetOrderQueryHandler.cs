using INFRA.Repositories;
using MediatR;

namespace APPLICATION.Order.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, IEnumerable<GetOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<GetOrderResponse>> Handle(GetOrderQuery command, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(x => x.Status != DOMAIN.Enums.OrderStatus.Finished);
        return GetOrderResponse.ToResponse(orders);
    }
}
