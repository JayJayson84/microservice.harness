using MicroService.Example.Domain.Services;
using MicroService.Example.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroService.Example.Resolver;

public static class ServiceResolver
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IExampleService, ExampleService>();
    }
}