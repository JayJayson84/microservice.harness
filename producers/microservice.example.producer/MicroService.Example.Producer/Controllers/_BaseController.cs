using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Example.Producer.Controllers;

public abstract class BaseController : ControllerBase
{
    const string FALLBACK_RESPONSE = "No content was generated from the request.";

    private Lazy<ObjectResult> _fallbackResponse => new Lazy<ObjectResult>(StatusCode(StatusCodes.Status500InternalServerError, new
    {
        ResponseCode = StatusCodes.Status204NoContent,
        ResponseType = MediaTypeNames.Text.Plain,
        Response = FALLBACK_RESPONSE
    }));

    public ObjectResult FallbackResponse => _fallbackResponse.Value;

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> CheckEndpoint()
    {
        var taskCompletionSource = new TaskCompletionSource<IActionResult>();

        ObjectResult? result = null;

        try
        {
            result = StatusCode(StatusCodes.Status200OK, new
            {
                ResponseCode = StatusCodes.Status200OK,
                ResponseType = MediaTypeNames.Text.Plain,
                Response = "Endpoint is healthy."
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
        finally
        {
            taskCompletionSource.SetResult(result ?? FallbackResponse);
            result = null;
        }

        return taskCompletionSource.Task;
    }
}