using Confluent.Kafka;
using System.Text.Json;
using ShippingService;

public class KafkaConsumerService : BackgroundService
{
    private readonly string _topic = "order-shipped";
    private readonly string _bootstrapServers = "kafka:9092"; // docker içindeki Kafka adresi

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "shipping-service",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                var message = result.Message.Value;

                var evt = JsonSerializer.Deserialize<OrderShippedEvent>(message);
                Console.WriteLine($"[ShippingService] Order shipped: {evt?.OrderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ShippingService] Error: {ex.Message}");
            }
        }

        consumer.Close();
    }
}