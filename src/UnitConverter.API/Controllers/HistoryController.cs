using Microsoft.AspNetCore.Mvc;
using UnitConverter.Application.DTOs;
using UnitConverter.Application.Services;

namespace UnitConverter.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class HistoryController : ControllerBase
{
    private readonly HistoryService _history;

    public HistoryController(HistoryService history) => _history = history;

    /// <summary>
    /// return the most recent conversion history.
    /// </summary>

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ConversionHistoryDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ConversionHistoryDto>> GetHistory([FromQuery] int limit = 20)
    {
        var records = _history.GetRecent(limit);
        return Ok(records);
    }
}
