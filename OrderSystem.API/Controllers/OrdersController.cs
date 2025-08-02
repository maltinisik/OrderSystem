using OrderSystem.Application.UseCases;
using OrderSystem.Contracts;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repositories;

namespace OrderSystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrder _createOrder;
    private readonly ShipOrder _shipOrder;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(CreateOrder createOrder, ShipOrder shipOrder,IOrderRepository orderRepository)
    {
        _createOrder = createOrder;
        _shipOrder = shipOrder;
        _orderRepository = orderRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] List<OrderItemDto> items)
    {
        if (items == null || !items.Any())
            return BadRequest("Sipariş kalemi girilmelidir.");

        var orderId = await _createOrder.ExecuteAsync(items);
        
        return Ok(orderId);
    }
    
    [HttpPost("ship/{id}")]
    public async Task<IActionResult> Ship(Guid id)
    {
        var orderId = await _shipOrder.ExecuteAsync(id);
        
        return Ok(orderId);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

       // var orderList = await _orderRepository.GetAllAsync();
        
        var dto = new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList()
        };

        return Ok(dto);        
    }
}