// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderSystem.Application.Common.Events;
using OrderSystem.Application.UseCases;
using OrderSystem.Domain.Repositories;
using OrderSystem.Infrastructure.Messaging;
using OrderSystem.Infrastructure.Repositories;
using OrderSystem.Contracts.Events;
using OrderSystem.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

//builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<CreateOrder>();
builder.Services.AddScoped<ShipOrder>();
builder.Services.AddScoped<PayOrder>();

builder.Services.AddControllers();

builder.Services.AddSingleton<IKafkaEventPublisher<OrderCreatedEvent>>(
 new KafkaEventPublisher<OrderCreatedEvent>("kafka:9092","order-created")
);

builder.Services.AddSingleton<IKafkaEventPublisher<OrderShippedEvent>>(
 new KafkaEventPublisher<OrderShippedEvent>("kafka:9092","order-shipped")
);

//db connection
builder.Services.AddDbContext<OrderDbContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


//logger start
Log.Logger = new LoggerConfiguration()
 .Enrich.FromLogContext()
 .WriteTo.Console()
 .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
 .WriteTo.Seq("http://seq:80") // docker-compose kullanıyorsan seq:80 yaz
 .CreateLogger();

builder.Host.UseSerilog();
//logger end

//security

// Secret key (gerçekte config'den alınmalı)
var key = "b82fe40b63ac45f99c8a1f4f9e3cb16a52ea9b2ee38f4059";

builder.Services.AddAuthentication(options =>
 {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 })
 .AddJwtBearer(options =>
 {
  options.TokenValidationParameters = new TokenValidationParameters
  {
   ValidateIssuer = false, // Prod'da true
   ValidateAudience = false, // Prod'da true
   ValidateLifetime = true,
   ValidateIssuerSigningKey = true,
   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
  };
 });

builder.Services.AddAuthorization(); // [Authorize] için gerekli

var app = builder.Build();

app.UseAuthentication(); // MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run(); // <-- BU SATIR MUTLAKA OLMALI
