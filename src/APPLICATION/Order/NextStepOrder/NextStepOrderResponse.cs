using DOMAIN.Enums;

namespace APPLICATION.Order.NextStepOrder;

public class NextStepOrderResponse
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public Guid CustomerId { get; set; }
    public double Discount { get; set; }
    public bool IsPaid { get; set; }
    public static NextStepOrderResponse ToResponse(DOMAIN.Order order)
        => new NextStepOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Discount = order.Discount,
            IsPaid = order.IsPaid,
            Status = order.Status,
        };

}
