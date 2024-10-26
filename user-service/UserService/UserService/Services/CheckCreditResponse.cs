using Confluent.Kafka;

namespace UserService.Services;

public class CheckCreditResponse
{
    public void Produce(bool enoughCredit, int userId)
    {
        const string topic = "check-credit-response";

        var config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092"
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();
        producer.Produce(topic, new Message<string, string> { Key = userId.ToString(), Value = enoughCredit.ToString() },
            (deliveryReport) =>
            {
                Console.WriteLine(deliveryReport.Error.Code != ErrorCode.NoError
                    ? $"Failed to deliver message: {deliveryReport.Error.Reason}"
                    : $"Produced event to topic {topic}: key = {userId} value = {enoughCredit}");
            });

        producer.Flush(TimeSpan.FromSeconds(10));
    }
}