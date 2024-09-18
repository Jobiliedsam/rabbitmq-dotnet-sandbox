using Broker.RabbitMq.Infrastructure.Configuration;
using Broker.RabbitMq.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.RabbitMq.Infrastructure.Extensions;

public static class RabbitMqServiceExtension
{
    public static void AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<RabbitMqConfiguration>(a => configuration.GetSection(RabbitMqConfiguration.RabbitMq).Bind(a));\
        
        services.Configure<RabbitMqConfiguration>(configuration.GetSection(RabbitMqConfiguration.RabbitMq));
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
    }
}