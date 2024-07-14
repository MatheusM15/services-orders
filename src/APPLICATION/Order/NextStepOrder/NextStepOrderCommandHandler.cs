using INFRA.Repositories;
using MediatR;

namespace APPLICATION.Order.NextStepOrder;

public class NextStepOrderCommandHandler : IRequestHandler<NextStepOrderCommand, NextStepOrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public NextStepOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<NextStepOrderResponse> Handle(NextStepOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        order.MoveToNextStep();
        _orderRepository.Update(order);
        _orderRepository.SaveChangesAsync();

        return NextStepOrderResponse.ToResponse(order);
    }
}
