using Broker.RabbitMq.Infrastructure.Configuration;
using Broker.RabbitMq.Infrastructure.Extensions;
using Producer.RabbitMq.Worker;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHostedService<Worker>();
builder.Services.AddRabbitMqConfiguration(builder.Configuration);

var host = builder.Build();
host.Run();
