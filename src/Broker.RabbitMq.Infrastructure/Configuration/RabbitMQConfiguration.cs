namespace Broker.RabbitMq.Infrastructure.Configuration;

public class RabbitMqConfiguration
{
    public const string RabbitMq = "RabbitMqConfiguration";
    
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}