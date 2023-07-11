using MicroService.Example.Domain.Models;

namespace MicroService.Example.Domain.Orchestration;

public interface IExampleOrchestrator
{
    Task<ExampleResponseModel> GetResponseAsync(ExampleModel model);
}