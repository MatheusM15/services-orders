using DOMAIN.Entities;

namespace APPLICATION.builder;

public class OrderProductBuilder
{
    private int _quantity;
    private Guid _productId;
    private List<OrderProductIngredient> _ingredients { get; set; }

    public OrderProductBuilder SetQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderProductBuilder SetProductId(Guid productId)
    {
        _productId = productId;
        return this;
    }

    public OrderProductBuilder SetIngredients(List<OrderProductIngredient> ingredients)
    {
        _ingredients = ingredients;
        return this;
    }

    public OrderProduct Build()
    {
        return OrderProduct.CreateProduct(_productId, _quantity)
                                       .AddIngredients(_ingredients);
    }
}
