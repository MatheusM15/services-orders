using INFRA.Repositories;
using MediatR;

namespace APPLICATION.Order.GetOrdersGroupByStatus;

public class GetOrdersGroupByStatusQueryHandler : IRequestHandler<GetOrdersGroupByStatusQuery, GetOrdersGroupByStatusResponse>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrdersGroupByStatusQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrdersGroupByStatusResponse> Handle(GetOrdersGroupByStatusQuery request, CancellationToken cancellationToken)
    {
        var received = await _orderRepository.GetAllAsync(x => x.Status == DOMAIN.Enums.OrderStatus.Received);
        var inProgress = await _orderRepository.GetAllAsync(x => x.Status == DOMAIN.Enums.OrderStatus.InProgress);
        var ready = await _orderRepository.GetAllAsync(x => x.Status == DOMAIN.Enums.OrderStatus.Ready);

        return GetOrdersGroupByStatusResponse.ToResponse(received, inProgress, ready);
    }
}
