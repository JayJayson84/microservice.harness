using MassTransit;

namespace MicroService.Example.Consumers;

public class ExampleConsumerDefinition : ConsumerDefinition<ExampleConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ExampleConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}