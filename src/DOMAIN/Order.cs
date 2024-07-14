using DOMAIN.Base;
using DOMAIN.Entities;
using DOMAIN.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace DOMAIN;

public class Order : AggregateRoot
{
    public Order(Guid customerId, double discount, OrderStatus status)
    {
        CustomerId = customerId;
        Discount = discount;
        Status = status;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Number { get; private set; }
    [ForeignKey("CustomerId")]
    public Guid CustomerId { get; private set; }
    public double Discount { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsPaid { get; private set; } = false;
    public List<OrderProduct> Products { get; set; }

    // Methods

    public void MarkAsPaid()
    {
        IsPaid = true;
    }

    public Order SetIsPaid(bool isPaid)
    {
        IsPaid = isPaid;
        return this;
    }

    public Order AddProducts(IEnumerable<OrderProduct> products)
    {
        if (Products is null) Products = new List<OrderProduct>();
        Products.AddRange(products);
        return this;
    }

    public static Order CreateOrder(Guid customerId, double discount, OrderStatus status)
    {
        return new Order(customerId, discount, status);
    }

    public Order MoveToNextStep()
    {
        Status = GetNextStatus(Status);
        return this;
    }

    public bool IsLastStatus()
        => Status == OrderStatus.Finished;

    private OrderStatus GetNextStatus(OrderStatus currentStatus)
    {
        switch (currentStatus)
        {
            case OrderStatus.Received:
                return OrderStatus.InProgress;

            case OrderStatus.InProgress:
                return OrderStatus.Ready;

            case OrderStatus.Ready:
                return OrderStatus.Finished;

            case OrderStatus.Finished:
                return currentStatus;

            default:
                return currentStatus;
        }
    }


}