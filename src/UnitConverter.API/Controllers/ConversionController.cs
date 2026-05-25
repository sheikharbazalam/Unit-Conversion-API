using Microsoft.AspNetCore.Mvc;
using UnitConverter.Application.DTOs;
using UnitConverter.Application.Services;

namespace UnitConverter.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class ConversionController : ControllerBase
{
    private readonly ConversionService _service;

    public ConversionController(ConversionService service) => _service = service;

    /// <summary>Convert a single value from one unit to another unit</summary>
    [HttpPost("convert")]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<ConversionResponse> Convert([FromBody] ConversionRequest request)
    {
        var result = _service.Convert(request);
        return Ok(result);
    }

    /// <summary>convert multiple value in one request (we set max:20(lmt)).</summary>
    [HttpPost("convert/batch")]
    [ProducesResponseType(typeof(IEnumerable<BatchConversionResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<BatchConversionResultDto>> ConvertBatch(
        [FromBody] IReadOnlyList<ConversionRequest> requests)
    {
        if (requests.Count > 20)
            return BadRequest(new ProblemDetails
            {
                Title = "Batch Too Large",
                Detail = "A maximum of 20 conversions per batch is allowed.",
                Status = StatusCodes.Status400BadRequest
            });

        var results = requests.Select(req =>
        {
            try
            {
                var result = _service.Convert(req);
                return new BatchConversionResultDto(req.Value, req.From, req.To, result, null);
            }
            catch (Exception ex)
            {
                return new BatchConversionResultDto(req.Value, req.From, req.To, null, ex.Message);
            }
        });

        return Ok(results);
    }
}

public sealed record BatchConversionResultDto(
    double InputValue,
    string From,
    string To,
    ConversionResponse? Result,
    string? Error
);
