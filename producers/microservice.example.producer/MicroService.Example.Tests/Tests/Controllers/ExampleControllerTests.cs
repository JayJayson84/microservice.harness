using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Nodes;
using MassTransit;
using MicroService.Contracts;
using MicroService.Example.Producer.Controllers;
using MicroService.Example.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MicroService.Example.Tests.Controllers;

public class ExampleControllerTests
{

    private readonly Mock<IRequestClient<IExampleContract>> _mockRequestClient;
    private readonly Mock<MassTransit.Response<IExampleResponse>> _mockMassTransitExampleResponse;

    public ExampleControllerTests()
    {
        _mockRequestClient = new Mock<IRequestClient<IExampleContract>>();
        _mockMassTransitExampleResponse = new Mock<MassTransit.Response<IExampleResponse>>();
    }

    [Fact]
    public async Task GetResponse_GivenAValue_ReturnsAJsonPayloadWithPlainTextResponse()
    {
        // Arrange
        var expectedValue = ValueHelper.RandomString();
        var mockExampleResponse = Mock.Of<IExampleResponse>((p) =>
            p.ResponseCode == StatusCodes.Status200OK
         && p.ResponseType == MediaTypeNames.Text.Plain
         && p.Message == expectedValue);

        _mockMassTransitExampleResponse
            .Setup(e => e.Message)
            .Returns(mockExampleResponse);

        _mockRequestClient
            .Setup(e => e.GetResponse<IExampleResponse>(
                It.IsAny<object>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<RequestTimeout>()))
            .ReturnsAsync(_mockMassTransitExampleResponse.Object);

        // Act
        var sut = new ExampleController(_mockRequestClient.Object);
        var result = await sut.GetResponse(expectedValue);

        // Assert
        var actionResult =
        Assert.IsType<ObjectResult>(result);

        var jsonData = JsonSerializer.Serialize(actionResult.Value);
        var jsonObject = JsonObject.Parse(jsonData)!;
        var responseCode = jsonObject["ResponseCode"]!.GetValue<int>();
        var responseType = jsonObject["ResponseType"]!.GetValue<string>();
        var response = jsonObject["Response"]!.GetValue<string>();

        Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode);
        Assert.Equal(StatusCodes.Status200OK, responseCode);
        Assert.Equal(MediaTypeNames.Text.Plain, responseType);
        Assert.Equal(expectedValue, response);
    }

}