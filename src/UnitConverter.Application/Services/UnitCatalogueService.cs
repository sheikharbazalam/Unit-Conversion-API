using UnitConverter.Application.DTOs;
using UnitConverter.Application.Interfaces;
using UnitConverter.Domain.Enums;

namespace UnitConverter.Application.Services;

public sealed class UnitCatalogueService
{
    private readonly IUnitRepository _units;

    public UnitCatalogueService(IUnitRepository units) => _units = units;

    public IReadOnlyList<UnitSummaryDto> GetAll() =>
        _units.GetAll()
              .Select(ToDto)
              .OrderBy(u => u.Category)
              .ThenBy(u => u.DisplayName)
              .ToList();

    public IReadOnlyList<UnitSummaryDto> GetByCategory(string category)
    {
        if (!Enum.TryParse<ConversionCategory>(category, ignoreCase: true, out var cat))
            throw new ArgumentException($"Unknown category: '{category}'.");

        return _units.GetByCategory(cat).Select(ToDto).ToList();
    }

    public IReadOnlyList<string> GetCategories() =>
        _units.GetCategories()
              .Select(c => c.ToString())
              .Order()
              .ToList();

    private static UnitSummaryDto ToDto(Domain.Entities.UnitDefinition u) =>
        new(u.Key, u.DisplayName, u.Symbol, u.Category.ToString(), u.Aliases);
}
