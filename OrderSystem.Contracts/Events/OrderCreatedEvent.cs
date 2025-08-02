namespace OrderSystem.Contracts.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}