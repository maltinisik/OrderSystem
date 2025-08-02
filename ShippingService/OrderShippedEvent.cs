namespace ShippingService;

public class OrderShippedEvent
{
    public Guid OrderId { get; set; }
    public DateTime ShippedAt { get; set; }
}
