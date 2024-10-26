using Confluent.Kafka;

namespace WebApplication1.Services;

public class CheckCredit
{
    public void Produce(int userId)
    {
        const string topic = "check-credit";

        var config = new ProducerConfig()
        {
            BootstrapServers = "kafka:9092"
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();
        producer.Produce(topic, new Message<string, string> { Key = "userId", Value = userId.ToString() },
            (deliveryReport) =>
            {
                Console.WriteLine(deliveryReport.Error.Code != ErrorCode.NoError
                    ? $"Failed to deliver message: {deliveryReport.Error.Reason}"
                    : $"Produced event to topic {topic}: key = userId value = {userId}");
            });

        producer.Flush(TimeSpan.FromSeconds(10));
    }
}