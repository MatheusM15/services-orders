using MediatR;

namespace APPLICATION.Order.GetOrder;

public record GetOrderQuery() : IRequest<IEnumerable<GetOrderResponse>>
{
}
