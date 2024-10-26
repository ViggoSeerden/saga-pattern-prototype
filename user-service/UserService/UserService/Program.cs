using Confluent.Kafka;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:9092",
    GroupId = "users"
};

builder.Services.AddSingleton(new ConsumerBuilder<string, string>(consumerConfig).Build());
builder.Services.AddHostedService<CheckCredit>();

var app = builder.Build();

app.Run();
