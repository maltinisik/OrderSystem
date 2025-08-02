namespace OrderSystem.Contracts;

public class OrderDto
{
    public Guid Id { get; set; }

    public string Status { get; set; }
    
    public List<OrderItemDto> Items { get; set; } = new();

}