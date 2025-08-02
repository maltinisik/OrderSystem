using OrderSystem.Contracts.Events;

namespace OrderSystem.Application.Common.Events;

public interface IKafkaEventPublisher<TEvent>
{
    Task PublishAsync(TEvent evt);
}