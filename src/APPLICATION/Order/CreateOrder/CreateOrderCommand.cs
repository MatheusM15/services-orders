using DOMAIN.Entities;
using MediatR;

namespace APPLICATION.Order.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    double Discount,
    IEnumerable<OrderProduct> Product) : IRequest<CreateOrderResponse>;
