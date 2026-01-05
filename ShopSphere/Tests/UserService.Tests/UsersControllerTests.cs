using NUnit.Framework;
using UserService.Data;
using UserService.Controllers;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class UsersControllerTests
{
    [Test]
    public void CreateUser_Pass()
    {
        Assert.Pass("Placeholder test for user creation.");
    }

    [Test]
    public async Task CreateUser_ShouldAddUserToDb()
    {
        // Arrange
        var dbOptions = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    	var db = new UserDbContext(dbOptions);

    	var producerMock = new Moq.Mock<Confluent.Kafka.IProducer<Confluent.Kafka.Null, string>>();
    	var controller = new UsersController(db, producerMock.Object);
    	var user = new User(Guid.NewGuid(), "Alice", "alice@example.com");

    	// Act
    	var result = await controller.Create(user);

    	// Assert
    	var ok = result as OkObjectResult;
    	Assert.IsNotNull(ok);
    	Assert.AreEqual(user, db.Users.First());
    }
}
