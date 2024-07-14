namespace APPLICATION.Order.GetOrdersGroupByStatus;


public class GetOrdersGroupByStatusResponse
{
    public IEnumerable<OrderResponse> Received { get; set; }
    public IEnumerable<OrderResponse> InProgress { get; set; }
    public IEnumerable<OrderResponse> Ready { get; set; }

    public class OrderResponse
    {
        public Guid Id { get; set; }

        public static IEnumerable<OrderResponse> ToResponse(IEnumerable<DOMAIN.Order> orders)
            => orders.Select(x => new OrderResponse { Id = x.Id });
    }

    public static GetOrdersGroupByStatusResponse ToResponse(IEnumerable<DOMAIN.Order> received, IEnumerable<DOMAIN.Order> inProgress, IEnumerable<DOMAIN.Order> ready)
        => new GetOrdersGroupByStatusResponse
        {
            Ready = OrderResponse.ToResponse(ready),
            Received = OrderResponse.ToResponse(received),
            InProgress = OrderResponse.ToResponse(inProgress),
        };
}
