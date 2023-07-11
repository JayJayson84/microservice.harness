using MicroService.Example.Domain.Orchestration;
using MicroService.Example.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroService.Example.Resolver;

public static class OrchestrationResolver
{
    public static void AddOrchestrators(this IServiceCollection services)
    {
        services.AddTransient<IExampleOrchestrator, ExampleOrchestrator>();
    }
}