using UserService.Models;

namespace UserService.Services;

using Confluent.Kafka;

public class CheckCredit(IConsumer<string, string> consumer) : IHostedService
{
    private readonly User?[] _userList =
    [
        new User(1, "John", 100),
        new User(2, "Doe", 99),
    ];

    private CheckCreditResponse _response = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        consumer.Subscribe("check-credit");
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

                var user = Array.Find(_userList, user => user?.Id.ToString() == consumeResult.Message.Value);
                if (user != null)
                {
                    _response.Produce(user.Credit >= 100, user.Id);   
                }
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumer.Close();
        return Task.CompletedTask;
    }
}