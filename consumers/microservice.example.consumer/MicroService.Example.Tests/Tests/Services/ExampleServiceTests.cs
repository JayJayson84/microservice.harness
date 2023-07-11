using System;
using System.Threading;
using System.Threading.Tasks;
using MicroService.Example.Domain.Services;
using MicroService.Example.Orchestration;
using MicroService.Example.Tests.Helpers;
using Moq;
using Xunit;

namespace MicroService.Example.Tests.Services;

public class ExampleServiceTests
{

    private Mock<IExampleService> _mockExampleService;

    public ExampleServiceTests()
    {
        _mockExampleService = new Mock<IExampleService>();
    }

    [Fact]
    public async void GetValue_RequestCancellation_TaskIsCancelled()
    {
        // Arrange
        var model = ModelHelper.SetupExampleModel();

        _mockExampleService
            .Setup(x => x.GetValue(It.IsAny<CancellationToken>()))
            .Throws<OperationCanceledException>();

        // Act
        var sut = new ExampleOrchestrator(_mockExampleService.Object);
        var result = () => sut.GetResponseAsync(model);

        // Assert
        var ex = await
        Assert.ThrowsAsync<TaskCanceledException>(result);
        Assert.Equal(ex.Task!.Status, TaskStatus.Canceled);
    }

    [Fact]
    public async void AppendValue_RequestCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var model = ModelHelper.SetupExampleModel();

        _mockExampleService
            .Setup(x => x.GetValue(It.IsAny<CancellationToken>()))
            .ReturnsAsync(model.Value!);
        _mockExampleService
            .Setup(x => x.AppendValue(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws<OperationCanceledException>();

        // Act
        var sut = new ExampleOrchestrator(_mockExampleService.Object);
        var result = () => sut.GetResponseAsync(model);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(result);
    }

}