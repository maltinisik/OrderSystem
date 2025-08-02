using ShippingService;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<KafkaConsumerService>();
    });

var host = builder.Build();
host.Run();





