using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.Infrastructure.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task SaveAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Order order)
    {
        var existingOrderIndex = _orders.FindIndex(o => o.Id == order.Id);
        if (existingOrderIndex == -1)
        {
            throw new KeyNotFoundException($"ID'si {order.Id} olan sipariş bulunamadı.");
        }
        
        _orders[existingOrderIndex] = order;
        return Task.CompletedTask;

    }

    public Task<Order?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
    }

    public Task<IEnumerable<Order>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders);
    }
}