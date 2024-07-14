using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Enums;
using System.Reflection;
using System.Xml;

namespace APPLICATION.builder;

public class OrderBuilder
{
    private Guid _orderId;
    private Guid _customerId;
    private double _discount;
    private OrderStatus _status;
    private bool _isPaid;
    private IEnumerable<OrderProduct> _products;

    public OrderBuilder SetCustomerId(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderBuilder SetDiscount(double discount)
    {
        _discount = discount;
        return this;
    }

    public OrderBuilder SetStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public OrderBuilder SetIsPaid(bool isPaid)
    {
        _isPaid = isPaid;
        return this;
    }

    public OrderBuilder AddProduct(IEnumerable<OrderProduct> products)
    {
        _products = products;
        return this;
    }

    public OrderBuilder SetId(Guid id)
    {
        _orderId = id;
        return this;
    }

    public DOMAIN.Order Build()
    {
        var order = new DOMAIN.Order(_customerId, _discount, _status)
            .SetIsPaid(_isPaid).AddProducts(_products);

        PropertyInfo propertyInfo = order.GetType().GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        if (propertyInfo != null)
        {
            propertyInfo.SetValue(order, _orderId);
        }

        return order;

    }
}
