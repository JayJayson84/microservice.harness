using System.Net;
using System.Net.Mime;
using System.Text.Json;
using AutoMapper;
using MassTransit;
using MicroService.Contracts;
using MicroService.Example.Domain.Models;
using MicroService.Example.Domain.Orchestration;

namespace MicroService.Example.Consumers;

public class ExampleConsumer : IConsumer<IExampleContract>
{
    IMapper _mapper;
    IExampleOrchestrator _exampleOrchestrator;

    public ExampleConsumer(
        IMapper mapper,
        IExampleOrchestrator exampleOrchestrator)
    {
        _mapper = mapper;
        _exampleOrchestrator = exampleOrchestrator;
    }

    public async Task Consume(ConsumeContext<IExampleContract> context)
    {
        if (!context.TryGetMessage<IExampleContract>(out var contract))
        {
            throw new PayloadException($"Message type {nameof(IExampleContract)} could not be consumed from the context.");
        }

        try
        {
            var domainModel = _mapper.Map<ExampleModel>(contract.Message);
            var domainResponse = await _exampleOrchestrator.GetResponseAsync(domainModel);

            await context.RespondAsync<IExampleResponse>(new
            {
                ResponseCode = (int)HttpStatusCode.OK,
                ResponseType = MediaTypeNames.Text.Plain,
                Message = domainResponse?.Message
            });
        }
        catch (AggregateException ae)
        {
            var exceptionMessages = ae
                .Flatten()
                .InnerExceptions
                .Select(x => x.Message);
            var exceptionMessagesJson = JsonSerializer.Serialize(exceptionMessages);

            await context.RespondAsync<IExampleResponse>(new
            {
                ResponseCode = (int)HttpStatusCode.InternalServerError,
                ResponseType = MediaTypeNames.Application.Json,
                Message = exceptionMessagesJson
            });
        }
        catch (Exception ex)
        {
            await context.RespondAsync<IExampleResponse>(new
            {
                ResponseCode = (int)HttpStatusCode.InternalServerError,
                ResponseType = MediaTypeNames.Text.Plain,
                Message = ex.Message
            });
        }
    }
}