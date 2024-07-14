using APPLICATION.Order.GetByIdAsync;
using INFRA.Repositories;
using MediatR;

namespace APPLICATION.Order.GetById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.GetOrderByIdAsync(request.OrderId, x => GetOrderByIdResponse.ToResponse(x));
        return result;
    }
}
