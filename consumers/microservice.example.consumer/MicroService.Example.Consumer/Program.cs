using System.Reflection;
using MicroService.Example.Resolver;

namespace MicroService.Example.Consumer;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();

                services.AddMessageBroker(entryAssembly);
                services.AddOrchestrators();
                services.AddServices();
                services.AddMappers();
            });
}