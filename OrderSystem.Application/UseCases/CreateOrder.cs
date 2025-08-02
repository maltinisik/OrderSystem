using OrderSystem.Application.Common.Events;
using OrderSystem.Contracts;
using OrderSystem.Contracts.Events;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.Application.UseCases;

public class CreateOrder
{
    private readonly IOrderRepository _orderRepository;
    private readonly IKafkaEventPublisher _eventPublisher;

    public CreateOrder(
        IOrderRepository orderRepository,
        IKafkaEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> ExecuteAsync(List<OrderItemDto> items)
    {
        var domainItems = items.Select(dto => new OrderItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        }).ToList();

        var order = Order.Create(domainItems);
        await _orderRepository.SaveAsync(order);

        var evt = new OrderCreatedEvent
        {
            OrderId = order.Id,
            Items = items
        };
        
        await _eventPublisher.PublishAsync(evt);

        return order.Id;
    }
}