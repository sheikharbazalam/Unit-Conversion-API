using Microsoft.AspNetCore.Mvc;
using UnitConverter.Application.DTOs;
using UnitConverter.Application.Services;

namespace UnitConverter.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class UnitsController : ControllerBase
{
    private readonly UnitCatalogueService _catalogue;

    public UnitsController(UnitCatalogueService catalogue) => _catalogue = catalogue;

    /// <summary>Returns all supported units, optionally filtered by category.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnitSummaryDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<UnitSummaryDto>> GetUnits([FromQuery] string? category = null)
    {
        var units = string.IsNullOrWhiteSpace(category)
            ? _catalogue.GetAll()
            : _catalogue.GetByCategory(category);

        return Ok(units);
    }

    /// <summary>Returns all supported conversion categories.</summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<string>> GetCategories() =>
        Ok(_catalogue.GetCategories());
}
