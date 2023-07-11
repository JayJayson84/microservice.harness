using MicroService.Example.Domain.Models;

namespace MicroService.Example.Tests.Helpers;

public static class ModelHelper
{
    public static ExampleModel SetupExampleModel() => new ExampleModel()
    {
        Value = ValueHelper.RandomString()
    };

    public static ExampleResponseModel SetupExampleResponseModel(ExampleModel model) => new ExampleResponseModel()
    {
        Message = $"{model.Value} - Continuation value"
    };
}