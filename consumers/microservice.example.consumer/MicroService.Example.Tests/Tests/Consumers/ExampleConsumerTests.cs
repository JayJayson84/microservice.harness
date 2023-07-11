using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using MicroService.Contracts;
using MicroService.Example.Consumers;
using MicroService.Example.Domain.Orchestration;
using MicroService.Example.Domain.Services;
using MicroService.Example.Mappers;
using MicroService.Example.Orchestration;
using MicroService.Example.Services;
using MicroService.Example.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicroService.Example.Tests.Consumers;

public class ExampleConsumerTests
{

    [Fact]
    public async Task ExampleConsumer_GivenContract_ShouldConsumeContractAndSendResponse()
    {
        // Arrange
        var assembly = AssemblyInfo.GetAssembly();
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<ExampleConsumer>();
            })
            .AddTransient<IExampleOrchestrator, ExampleOrchestrator>()
            .AddTransient<IExampleService, ExampleService>()
            .AddAutoMapper(assembly)
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        var consumerHarness = harness.GetConsumerHarness<ExampleConsumer>();

        await harness.Start();

        var client = harness.GetRequestClient<IExampleContract>();

        // Act
        var response = await client.GetResponse<IExampleResponse>(new
        {
            Value = ValueHelper.RandomString()
        });

        // Assert
        Assert.True(await harness.Sent.Any<IExampleResponse>());
        Assert.True(await harness.Consumed.Any<IExampleContract>());
        Assert.True(await consumerHarness.Consumed.Any<IExampleContract>());
    }

}