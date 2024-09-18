using RabbitMQ.Client;

namespace Broker.RabbitMq.Infrastructure.Services;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}