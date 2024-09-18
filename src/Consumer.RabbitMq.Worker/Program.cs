using Broker.RabbitMq.Infrastructure.Extensions;
using Consumer.RabbitMq.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddRabbitMqConfiguration(builder.Configuration);

var host = builder.Build();
host.Run();
