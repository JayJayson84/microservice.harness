using AutoMapper;
using MicroService.Contracts;
using MicroService.Example.Domain.Models;
using MicroService.Example.Mappers;
using MicroService.Example.Tests.Helpers;
using Moq;
using Xunit;

namespace MicroService.Example.Tests.Consumers;

public class ExampleMapperTests
{

    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<ExampleMapperProfile>();
    }));

    [Fact]
    public void ExampleMapper_GivenIExampleContract_MapsToExampleModel()
    {
        // Arrange
        var value = ValueHelper.RandomString();
        var exampleContract = Mock.Of<IExampleContract>(e => e.Value == value);

        // Act
        var result = _mapper.Map<ExampleModel>(exampleContract);

        // Assert
        Assert.Equal(exampleContract.Value, result.Value);
    }

}