namespace MicroService.Example.Domain.Services;

public interface IExampleService
{
    Task<string> GetValue(CancellationToken cancellationToken = default(CancellationToken));
    Task<string> AppendValue(string value, CancellationToken cancellationToken = default(CancellationToken));
}