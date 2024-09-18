using Broker.RabbitMq.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Broker.RabbitMq.Infrastructure.Services;

public class RabbitMqService(IOptions<RabbitMqConfiguration> rabbitMqConfiguration) : IRabbitMqService
{
    private readonly RabbitMqConfiguration _rabbitMqConfiguration = rabbitMqConfiguration.Value;

    public IConnection CreateChannel()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitMqConfiguration.Host,
            UserName = _rabbitMqConfiguration.UserName,
            Password = _rabbitMqConfiguration.Password,
            DispatchConsumersAsync = true //Set asynchronous message handlers to true.
        };
        
        var channel = connectionFactory.CreateConnection();
        return channel;
    }
}