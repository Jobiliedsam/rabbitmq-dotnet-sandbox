using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.RabbitMq.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
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
            
            await Task.Delay(10000, stoppingToken);
        }
    }

    private void ConsumerMessage()
    {
        var factory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" Received: {message}");
        };
        
        channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
    }
}
