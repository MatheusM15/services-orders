using APPLICATION.Order.GetOrder;

namespace API.Requets.Order;

public class GetOrderRequest
{
    public GetOrderQuery ToQuery() => new();
}
