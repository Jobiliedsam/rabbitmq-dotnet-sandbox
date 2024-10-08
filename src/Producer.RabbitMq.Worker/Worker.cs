using System.Text;
using Broker.RabbitMq.Infrastructure.Services;
using RabbitMQ.Client;

namespace Producer.RabbitMq.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;

    public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService)
    {
        _logger = logger;
        _connection = rabbitMqService.CreateChannel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            
            _logger.LogInformation("Start Producer Message");
            
            ProducerMessage();
            
            _logger.LogInformation("Finish Producer Message");
            
            await Task.Delay(10000, stoppingToken);
        }
    }

    private void ProducerMessage()
    {
        using var channel = _connection.CreateModel();
        
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
        
        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);
        
        channel.BasicPublish(exchange: string.Empty, routingKey: "hello", basicProperties: null, body: body);

        Console.WriteLine(" [x] Sent {0}", message);
    }
}
