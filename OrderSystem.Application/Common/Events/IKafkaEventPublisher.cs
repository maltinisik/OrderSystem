using OrderSystem.Contracts.Events;

namespace OrderSystem.Application.Common.Events;

public interface IKafkaEventPublisher
{
    Task PublishAsync(OrderCreatedEvent evt);
}