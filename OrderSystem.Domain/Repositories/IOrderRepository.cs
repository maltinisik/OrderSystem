using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Repositories;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    
    Task UpdateAsync(Order order);
    
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();    
}