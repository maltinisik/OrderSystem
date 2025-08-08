using OrderSystem.Domain.Enums;

namespace OrderSystem.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    public List<OrderItem> Items { get; private set; }

    public OrderStatus Status { get; set; } = OrderStatus.Created;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private Order()
    {
        // Items zaten yukarıda boş liste olarak başlatıldı
    }
    
    public Order(List<OrderItem> items)
    {
        Items = items;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Only Newly Created Order is allowed! Status=" + Status);

        Status = OrderStatus.Paid;
    }
    
    public bool isPaid()
    {
        return Status == OrderStatus.Paid;
    }
    
    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Only Paid Order is allowed! Status=" + Status);

        Status = OrderStatus.Shipped;
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