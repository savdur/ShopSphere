
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Events;
using UserService.Messaging;
using UserService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserDbContext _db;
    private readonly KafkaProducer _producer;

    public UsersController(UserDbContext db, KafkaProducer producer)
    {
        _db = db;
        _producer = producer;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
            return BadRequest("Email is required");

        user.Id = Guid.NewGuid();
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        await _producer.PublishAsync("user.created",
            new UserCreatedEvent(user.Id, user.Name, user.Email));

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _db.Users.Find(id);
        return user == null ? NotFound() : Ok(user);
    }
}
