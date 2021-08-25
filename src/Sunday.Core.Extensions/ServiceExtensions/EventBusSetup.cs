using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sunday.Core.Application.IntergrationEvents;
using Sunday.Core.EventBus;
using Sunday.Core.Infrastructure;

namespace Sunday.Core.Extensions
{
    /// <summary>
    /// EventBus 事件总线服务
    /// </summary>
    public static class EventBusSetup
    {
        public static void AddEventBusSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.App(new string[] { "EventBus", "Enabled" }).ObjToBool())
            {
                var subscriptionClientName = Appsettings.App(new string[] { "EventBus", "SubscriptionClientName" });

                services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
                services.AddTransient<BlogDeletedIntegrationEventHandler>();

                if (Appsettings.App(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
                {
                    services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                    {
                        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                        var retryCount = 5;
                        if (!string.IsNullOrEmpty(Appsettings.App(new string[] { "RabbitMQ", "RetryCount" })))
                        {
                            retryCount = int.Parse(Appsettings.App(new string[] { "RabbitMQ", "RetryCount" }));
                        }

                        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                    });
                }
                if (Appsettings.App(new string[] { "Kafka", "Enabled" }).ObjToBool())
                {
                    services.AddHostedService<KafkaConsumerHostService>();
                    services.AddSingleton<IEventBus, EventBusKafka>();
                }
            }
        }

        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            if (Appsettings.App(new string[] { "EventBus", "Enabled" }).ObjToBool())
            {
                var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

                eventBus.Subscribe<BlogDeletedIntegrationEvent, BlogDeletedIntegrationEventHandler>();
            }
        }
    }
}