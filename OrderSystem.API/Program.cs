// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrderSystem.Application.Common.Events;
using OrderSystem.Application.UseCases;
using OrderSystem.Domain.Repositories;
using OrderSystem.Infrastructure.Messaging;
using OrderSystem.Infrastructure.Repositories;
using OrderSystem.Contracts.Events;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<CreateOrder>();
builder.Services.AddScoped<ShipOrder>();

builder.Services.AddControllers();

builder.Services.AddSingleton<IKafkaEventPublisher<OrderCreatedEvent>>(
 new KafkaEventPublisher<OrderCreatedEvent>("kafka:9092","order-created")
);

builder.Services.AddSingleton<IKafkaEventPublisher<OrderShippedEvent>>(
 new KafkaEventPublisher<OrderShippedEvent>("kafka:9092","order-shipped")
);

//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var app = builder.Build();

app.MapControllers();
app.Run(); // <-- BU SATIR MUTLAKA OLMALI