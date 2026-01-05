
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace UserService.Messaging;

public class UserEventConsumer : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "user-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("order.created");

            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = consumer.Consume(stoppingToken);
                Console.WriteLine($"[UserService] Order event received: {msg.Message.Value}");
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
