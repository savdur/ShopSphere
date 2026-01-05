
namespace OrderService.Events;

public record OrderCreatedEvent(Guid Id, Guid UserId, decimal Price);
