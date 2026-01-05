
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace OrderService.Messaging;

public class OrderEventConsumer : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "order-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("user.created");

            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = consumer.Consume(stoppingToken);
                Console.WriteLine($"[OrderService] User event received: {msg.Message.Value}");
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
