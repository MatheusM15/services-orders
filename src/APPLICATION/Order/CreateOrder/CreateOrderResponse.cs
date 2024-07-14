namespace APPLICATION.Order.CreateOrder;

public class CreateOrderResponse
{
    public Guid Id { get; set; }

    public static CreateOrderResponse ToResponse(DOMAIN.Order order)
        => new CreateOrderResponse
        {
            Id = order.Id,
        };
}
