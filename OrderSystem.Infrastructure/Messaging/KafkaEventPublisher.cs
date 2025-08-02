using System.Text.Json;
using Confluent.Kafka;
using OrderSystem.Application.Common.Events;
using OrderSystem.Contracts.Events;

namespace OrderSystem.Infrastructure.Messaging;

public class KafkaEventPublisher : IKafkaEventPublisher
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic = "order-created";

    public KafkaEventPublisher(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync(OrderCreatedEvent evt)
    {
        var message = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(evt)
        };
        
        Console.WriteLine($"[Kafka] Mesaj gönderiliyor: {message}"); 

        await _producer.ProduceAsync(_topic, message);
    }
}