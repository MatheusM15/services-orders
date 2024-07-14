using DOMAIN.Enums;

namespace APPLICATION.Order.GetOrder;

public class GetOrderResponse
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public Guid CustomerId { get; set; }
    public double Discount { get; set; }
    public bool IsPaid { get; set; }


    public static IEnumerable<GetOrderResponse> ToResponse(IEnumerable<DOMAIN.Order> orders)
        => orders.Select(x => new GetOrderResponse
        {
            Id = x.Id,
            CustomerId = x.CustomerId,
            Discount = x.Discount,
            IsPaid = x.IsPaid,
            Status = x.Status,
        });
}
