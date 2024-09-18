using System.Text;
using Broker.RabbitMq.Infrastructure.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.RabbitMq.Worker;

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
            _logger.LogInformation("Start Consumer Message");

            ConsumerMessage();
            
            _logger.LogInformation("Finish Consumer Message");
            
            await Task.Delay(20000, stoppingToken);
        }
    }

    private void ConsumerMessage()
    {
        using var channel = _connection.CreateModel();
        
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" Received: {message}");
            await Task.Yield();
        };
        
        channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
    }
}
