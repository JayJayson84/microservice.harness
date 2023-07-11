using System;
using System.Threading;
using MicroService.Example.Domain.Models;
using MicroService.Example.Domain.Services;
using MicroService.Example.Orchestration;
using MicroService.Example.Tests.Helpers;
using Moq;
using Xunit;

namespace MicroService.Example.Tests.Orchestration;

public class ExampleOrchestratorTests
{

    private Mock<IExampleService> _mockExampleService;

    public ExampleOrchestratorTests()
    {
        _mockExampleService = new Mock<IExampleService>();
    }

    [Fact]
    public async void GetResponseAsync_ModelIsValid_HasResponseMessage()
    {
        // Arrange
        var model = ModelHelper.SetupExampleModel();
        var responseModel = ModelHelper.SetupExampleResponseModel(model);

        _mockExampleService
            .Setup(x => x.GetValue(It.IsAny<CancellationToken>()))
            .ReturnsAsync(model.Value!);
        _mockExampleService
            .Setup(x => x.AppendValue(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseModel.Message!);

        // Act
        var sut = new ExampleOrchestrator(_mockExampleService.Object);
        var result = await sut.GetResponseAsync(model);

        // Assert
        Assert.Equal(responseModel.Message, result.Message);
    }

    [Fact]
    public async void GetResponseAsync_ModelIsNull_ThrowsArgumentException()
    {
        // Arrange
        ExampleModel model = null!;

        // Act
        var sut = new ExampleOrchestrator(_mockExampleService.Object);
        var result = () => sut.GetResponseAsync(model);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }

}