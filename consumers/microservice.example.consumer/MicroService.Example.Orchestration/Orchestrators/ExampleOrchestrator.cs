using MicroService.Example.Domain.Models;
using MicroService.Example.Domain.Orchestration;
using MicroService.Example.Domain.Services;

namespace MicroService.Example.Orchestration;

public class ExampleOrchestrator : IExampleOrchestrator
{
    IExampleService _exampleService;

    public ExampleOrchestrator(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    public Task<ExampleResponseModel> GetResponseAsync(ExampleModel model)
    {
        if (model == null) throw new ArgumentException(nameof(model));

        return GetResponseAsyncInternal(model);
    }

    private async Task<ExampleResponseModel> GetResponseAsyncInternal(ExampleModel model)
    {
        var cancellationTimeout = TimeSpan.FromSeconds(30);
        var cancellationTokenSource = new CancellationTokenSource(cancellationTimeout);
        var cancellationToken = cancellationTokenSource.Token;

        return await Task.Run(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _exampleService.GetValue(cancellationToken);
        },
        cancellationToken).ContinueWith(async (antecedent) =>
        {
            var value = antecedent.Result;

            if (string.IsNullOrEmpty(value)) cancellationTokenSource.Cancel();
            cancellationToken.ThrowIfCancellationRequested();

            value = await _exampleService.AppendValue(value, cancellationToken);

            return new ExampleResponseModel()
            {
                Message = value
            };
        },
        cancellationToken,
        TaskContinuationOptions.OnlyOnRanToCompletion,
        TaskScheduler.Current).Unwrap();
    }
}