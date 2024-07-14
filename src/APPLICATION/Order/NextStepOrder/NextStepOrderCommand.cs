using MediatR;

namespace APPLICATION.Order.NextStepOrder;

public record NextStepOrderCommand(
    Guid OrderId
    ) : IRequest<NextStepOrderResponse>;
