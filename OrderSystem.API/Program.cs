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

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<CreateOrder>();

builder.Services.AddControllers();

builder.Services.AddSingleton<IKafkaEventPublisher>(
 new KafkaEventPublisher("kafka:9092")
);

//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var app = builder.Build();

app.MapControllers();
app.Run(); // <-- BU SATIR MUTLAKA OLMALI