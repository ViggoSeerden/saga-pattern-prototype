using System.Collections.Concurrent;

namespace WebApplication1.Services;
using Confluent.Kafka;

public class CheckCreditResponse(IConsumer<string, string> consumer)
    : IHostedService
{
    private static ConcurrentDictionary<int, bool> _creditCheckResults = new ConcurrentDictionary<int, bool>();
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        consumer.Subscribe("check-credit-response");
        Task.Run(() =>
        {
            Console.WriteLine("Now listening...");
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                if (consumeResult is null)
                {
                    return;
                }

                Console.WriteLine(consumeResult.Message.Key);
                Console.WriteLine(consumeResult.Message.Value);

                // orderService.SetCreditCheckResult(int.Parse(consumeResult.Message.Key), bool.Parse(consumeResult.Message.Value));
                
                int userId = int.Parse(consumeResult.Message.Key);
                bool hasCredit = bool.Parse(consumeResult.Message.Value);
                
                _creditCheckResults[userId] = hasCredit;
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumer.Close();
        return Task.CompletedTask;
    }
    
    public static bool? GetCreditCheckResult(int userId)
    {
        _creditCheckResults.TryGetValue(userId, out bool result);
        return result; // Return null if not found
    }
}