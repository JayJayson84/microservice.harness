using MicroService.Example.Domain.Services;

namespace MicroService.Example.Services;

public class ExampleService : IExampleService
{
    public async Task<string> GetValue(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return "Service value";
        },
        cancellationToken);
    }

    public async Task<string> AppendValue(string value, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return $"{value} - Continuation value";
        },
        cancellationToken);
    }
}