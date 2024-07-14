using MediatR;

namespace APPLICATION.Order.GetOrdersGroupByStatus;

public record GetOrdersGroupByStatusQuery : IRequest<GetOrdersGroupByStatusResponse>;

