using MicroService.Example.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace MicroService.Example.Resolver;

public static class MapperResolver
{
    public static void AddMappers(this IServiceCollection services)
    {
        var assembly = AssemblyInfo.GetAssembly();
        services.AddAutoMapper(assembly);
    }
}