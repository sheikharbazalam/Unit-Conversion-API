using UnitConverter.Application.Interfaces;
using UnitConverter.Domain.Entities;
using UnitConverter.Domain.Enums;
using UnitConverter.Infrastructure.Data;

namespace UnitConverter.Infrastructure.Repositories;

public sealed class InMemoryUnitRepository : IUnitRepository
{
    private readonly Dictionary<string, UnitDefinition> _lookup;
    private readonly IReadOnlyList<UnitDefinition> _all;

    public InMemoryUnitRepository()
    {
        _all = UnitDefinitionSeed.All;
        _lookup = new Dictionary<string, UnitDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var unit in _all)
        {
            _lookup[unit.Key] = unit;
            foreach (var alias in unit.Aliases)
                _lookup.TryAdd(alias, unit);
        }
    }

    public UnitDefinition? FindUnit(string keyOrAlias) =>
        _lookup.TryGetValue(keyOrAlias.Trim(), out var unit) ? unit : null;

    public IReadOnlyList<UnitDefinition> GetByCategory(ConversionCategory category) =>
        _all.Where(u => u.Category == category).ToList();

    public IReadOnlyList<ConversionCategory> GetCategories() =>
        _all.Select(u => u.Category).Distinct().ToList();

    public IReadOnlyList<UnitDefinition> GetAll() => _all;
}
