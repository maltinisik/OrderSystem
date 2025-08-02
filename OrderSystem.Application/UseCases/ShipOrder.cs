using OrderSystem.Application.Common.Events;
using OrderSystem.Contracts;
using OrderSystem.Contracts.Events;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.Application.UseCases;

public class ShipOrder
{
    private readonly IOrderRepository _orderRepository;
    private readonly IKafkaEventPublisher<OrderShippedEvent> _eventPublisher;

    public ShipOrder(
        IOrderRepository orderRepository,
        IKafkaEventPublisher<OrderShippedEvent> eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> ExecuteAsync(Guid orderId)
    {

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception($"Order with Id {orderId} not found.");
        
        order.MarkAsShipped();
        await _orderRepository.UpdateAsync(order);

        var evt = new OrderShippedEvent()
        {
            OrderId = order.Id,
            ShippedAt = DateTime.UtcNow
        };
        
        await _eventPublisher.PublishAsync(evt);

        return order.Id;
    }
}