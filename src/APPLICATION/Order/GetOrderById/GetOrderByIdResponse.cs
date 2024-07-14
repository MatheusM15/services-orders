using APPLICATION.Order.GetOrderById;
using DOMAIN.Enums;

namespace APPLICATION.Order.GetById;

public class GetOrderByIdResponse
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public Guid CustomerId { get; set; }
    public double Discount { get; set; }
    public bool IsPaid { get; set; }
    public IEnumerable<ProductsResponse> Products { get; set; }

    public static GetOrderByIdResponse ToResponse(DOMAIN.Order order)
        => new GetOrderByIdResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Discount = order.Discount,
            IsPaid = order.IsPaid,
            Status = order.Status,
            Products = ProductsResponse.ToResponse(order.Products)
        };
}
