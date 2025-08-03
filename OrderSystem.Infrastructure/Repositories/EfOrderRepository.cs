using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.Infrastructure.Repositories;

public class EfOrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public EfOrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Order updatedOrder)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);

        if (existingOrder is null)
            return;

        // Örnek: Mevcut item'ları temizleyip yeni item'ları ekleme
        //_context.OrderItems.RemoveRange(existingOrder.Items);
        //existingOrder.Items = updatedOrder.Items;

        existingOrder.MarkAsShipped();
        
        await _context.SaveChangesAsync();
    }    

    
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public Task<IEnumerable<Order>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_context.Orders);
    }    
}