using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MicroService.Example.Resolver;

public static class MessageBrokerResolver
{
    public static void AddMessageBroker(this IServiceCollection services, Assembly? entryAssembly)
    {
        services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();

            x.SetKebabCaseEndpointNameFormatter();

            // By default, sagas are in-memory, but should be changed to a durable
            // saga repository.
            x.SetInMemorySagaRepositoryProvider();

            x.AddConsumers(entryAssembly);
            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "broker", h =>
                {
                    h.Username("broker");
                    h.Password("*wl*Lf6PiJ7s3z94%du7@6eLa");
                });

                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}