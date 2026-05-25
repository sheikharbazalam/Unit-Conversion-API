using Microsoft.AspNetCore.Mvc;

namespace UnitConverter.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Return 200 ok when service for this is running, also return some basic info about the service like version and timestamp.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get() =>
        Ok(new HealthResponse("healthy", DateTimeOffset.UtcNow, "1.0.0"));
}

public sealed record HealthResponse(string Status, DateTimeOffset Timestamp, string Version);
