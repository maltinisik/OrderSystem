using OrderSystem.Domain.Enums;

namespace OrderSystem.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    public List<OrderItem> Items { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order(List<OrderItem> items)
    {
        Items = items;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Only Newly Created Order is allowed");

        Status = OrderStatus.Paid;
    }
    
    public void Cancel()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid order");

        Status = OrderStatus.Cancelled;
    }

    public static Order Create(List<OrderItem> domainItems)
    {
        var order = new Order(domainItems);
        
        return order;
    }
}