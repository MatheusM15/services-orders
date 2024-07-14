using DOMAIN.Entities;

namespace APPLICATION.Order.GetOrderById;

public class ProductsResponse
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public static IEnumerable<ProductsResponse> ToResponse(IEnumerable<OrderProduct> products)
        => products.Select(x => new ProductsResponse
        {
            ProductId = x.ProductId,
            Quantity = x.Quantity,
        });
}
