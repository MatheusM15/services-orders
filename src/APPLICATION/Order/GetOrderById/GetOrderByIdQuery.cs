using APPLICATION.Order.GetById;
using MediatR;

namespace APPLICATION.Order.GetByIdAsync;

public record GetOrderByIdQuery(
    Guid OrderId) : IRequest<GetOrderByIdResponse>;
