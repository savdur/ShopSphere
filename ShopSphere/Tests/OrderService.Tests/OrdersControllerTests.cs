using NUnit.Framework;
using OrderService.Data;
using OrderService.Controllers;
using Microsoft.EntityFrameworkCore;


[TestFixture]
public class OrdersControllerTests
{
    [Test]
    public void CreateOrder_Pass()
    {
        Assert.Pass("Placeholder test for order creation.");
    }

    [Test]
    public async Task CreateOrder_ShouldAddUserToDb()
    {
        // Arrange
        var dbOptions = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    	var db = new OrderDbContext(dbOptions);
    	
        var producerMock = new Moq.Mock<Confluent.Kafka.IProducer<Confluent.Kafka.Null, string>>();
    	var controller = new OrdersController(db, producerMock.Object);
    	var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "Milk", 3, 5.30m);

    	// Act
    	var result = await controller.Create(order);

    	// Assert
    	var ok = result as OkObjectResult;
    	Assert.IsNotNull(ok);
    	Assert.AreEqual(order, db.Orders.First());
    }
}
