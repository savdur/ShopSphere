
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Events;
using OrderService.Messaging;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _db;
    private readonly KafkaProducer _producer;

    public OrdersController(OrderDbContext db, KafkaProducer producer)
    {
        _db = db;
        _producer = producer;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        if (order.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero");

        order.Id = Guid.NewGuid();
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _producer.PublishAsync("order.created",
            new OrderCreatedEvent(order.Id, order.UserId, order.Price));

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var order = _db.Orders.Find(id);
        return order == null ? NotFound() : Ok(order);
    }
}
