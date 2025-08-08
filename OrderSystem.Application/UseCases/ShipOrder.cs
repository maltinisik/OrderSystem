using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ShipOrder> _logger;


    public ShipOrder(
        IOrderRepository orderRepository,
        IKafkaEventPublisher<OrderShippedEvent> eventPublisher,
        ILogger<ShipOrder> logger)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Guid> ExecuteAsync(Guid orderId)
    {

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception($"Order with Id {orderId} not found.");

        if (order.isPaid() == false)
        {
            throw new Exception($"Order with Id {orderId} not paid.");            
        }
        
        _logger.LogInformation("Order Status: "+ order.Status);
        order.MarkAsShipped();
        
        _logger.LogInformation("Order Status: "+ order.Status);
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