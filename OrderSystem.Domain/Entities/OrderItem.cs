namespace OrderSystem.Domain.Entities;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}