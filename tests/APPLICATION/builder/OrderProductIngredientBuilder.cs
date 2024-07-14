using DOMAIN.Entities;

namespace APPLICATION.builder;

public class OrderProductIngredientBuilder
{
    private int _quantity;
    private Guid _id;

    public OrderProductIngredientBuilder SetQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }
    public OrderProductIngredientBuilder SetId(Guid id)
    {
        _id = id;
        return this;
    }

    public OrderProductIngredient Build()
    {
        return OrderProductIngredient.CreateIngredient(_quantity);
    }
}
