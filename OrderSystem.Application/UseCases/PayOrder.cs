using OrderSystem.Application.Common.Events;
using OrderSystem.Contracts;
using OrderSystem.Contracts.Events;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.Application.UseCases;

public class PayOrder
{
    private readonly IOrderRepository _orderRepository;

    public PayOrder(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> ExecuteAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception($"Order with Id {orderId} not found.");
        
        order.MarkAsPaid();
        await _orderRepository.UpdateAsync(order);
        
        return order.Id;
    }
}