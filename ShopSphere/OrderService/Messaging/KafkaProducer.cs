
using Confluent.Kafka;
using System.Text.Json;

namespace OrderService.Messaging;

public class KafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092"
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync(string topic, object data)
    {
        var json = JsonSerializer.Serialize(data);
        await _producer.ProduceAsync(topic,
            new Message<string, string> { Value = json });
    }
}
