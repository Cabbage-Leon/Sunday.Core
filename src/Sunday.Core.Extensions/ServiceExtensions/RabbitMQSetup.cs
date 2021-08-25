using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sunday.Core.EventBus;
using Sunday.Core.Infrastructure;
using RabbitMQ.Client;

namespace Sunday.Core.Extensions
{
    /// <summary>
    /// RBMQ 启动服务
    /// </summary>
    public static class RabbitMQSetup
    {
        public static void AddRabbitMQSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.App(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                   {
                       var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

                       var factory = new ConnectionFactory()
                       {
                           HostName = Appsettings.App(new string[] { "RabbitMQ", "Connection" }),
                           DispatchConsumersAsync = true
                       };

                       if (!string.IsNullOrEmpty(Appsettings.App(new string[] { "RabbitMQ", "UserName" })))
                       {
                           factory.UserName = Appsettings.App(new string[] { "RabbitMQ", "UserName" });
                       }

                       if (!string.IsNullOrEmpty(Appsettings.App(new string[] { "RabbitMQ", "Password" })))
                       {
                           factory.Password = Appsettings.App(new string[] { "RabbitMQ", "Password" });
                       }

                       var retryCount = 5;
                       if (!string.IsNullOrEmpty(Appsettings.App(new string[] { "RabbitMQ", "RetryCount" })))
                       {
                           retryCount = int.Parse(Appsettings.App(new string[] { "RabbitMQ", "RetryCount" }));
                       }

                       return new RabbitMQPersistentConnection(factory, logger, retryCount);
                   });
            }
        }
    }
}