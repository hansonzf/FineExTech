using EventBus.Interfaces;
using EventBus.RabbitMQ;
using EventBus;
using RabbitMQ.Client;
using Warehouse.Api.Application.IntegrationEvents.EventHandling;
using Warehouse.Api.Application.IntegrationEvents.Events;

namespace Warehouse.Api
{
    public static class EventBusConfiguration
    {
        public static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<CargoArrivedStorehouseIntegrationEventHandler, CargoArrivedStorehouseIntegrationEventHandler>();

            builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "admin",
                    Password = "admin",
                    DispatchConsumersAsync = true
                };

                var retryCount = 3;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(factory => {
                var subscriptionClientName = "warehouse-domain-queue";
                var rabbitMQPersistentConnection = factory.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = factory.CreateScope();
                var logger = factory.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = factory.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 3;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, factory, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });
            builder.Services.AddSingleton<IEventBusSubscriptionsManager, DefaultEventBusSubscriptionsManager>();
        }

        public static void UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<CargoArrivedStorehouseIntegrationEvent, CargoArrivedStorehouseIntegrationEventHandler>();
        }
    }
}
