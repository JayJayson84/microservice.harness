using System.Net.Mime;
using MassTransit;
using MicroService.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Example.Producer.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ExampleController : BaseController
{
    private readonly IRequestClient<IExampleContract> _requestClient;

    public ExampleController(IRequestClient<IExampleContract> requestClient)
    {
        _requestClient = requestClient;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetResponse(string? value)
    {
        ObjectResult? result = null;

        try
        {
            var contract = new { Value = value };
            var response = await _requestClient.GetResponse<IExampleResponse>(contract);
            var responseMessage = response.Message;

            result = StatusCode(StatusCodes.Status200OK, new
            {
                ResponseCode = responseMessage.ResponseCode,
                ResponseType = responseMessage.ResponseType,
                Response = responseMessage.Message
            });
        }
        catch (Exception ex)
        {
            result = StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ResponseCode = StatusCodes.Status500InternalServerError,
                ResponseType = MediaTypeNames.Text.Plain,
                Response = ex.Message
            });
        }

        return result ?? FallbackResponse;
    }
}