using APPLICATION.Order.CreateOrder;
using DOMAIN.Entities;

namespace API.Requets.Order;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public double Discount { get; set; }
    public IEnumerable<OrderProductRequest> OrdersProducts { get; set; }

    public class OrderProductRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<IngredientRequest> Ingredients { get; set; }

    }

    public class IngredientRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }

    private IEnumerable<OrderProduct> ToProductList(IEnumerable<OrderProductRequest> products)
    => products.Select(d => OrderProduct.CreateProduct(d.ProductId, d.Quantity).AddIngredients(ToIngredientsList(d.Ingredients)));

    private IEnumerable<OrderProductIngredient> ToIngredientsList(IEnumerable<IngredientRequest> ingredientRequests)
        => ingredientRequests.Select(x => OrderProductIngredient.CreateIngredient(x.Quantity));

    public CreateOrderCommand ToCommand() => new CreateOrderCommand(CustomerId, Discount, ToProductList(OrdersProducts));

}


